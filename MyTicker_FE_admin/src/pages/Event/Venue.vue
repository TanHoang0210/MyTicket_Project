<template>
    <div class="content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-12">
                    <card class="strpied-tabled-with-hover" body-classes="table-full-width table-responsive">
                        <template slot="header">
                            <div class="row" style="display: flex; justify-content: space-between;">
                                <div class="col-md-6">
                                    <h4 class="card-title">Danh sách sân vận động</h4>
                                </div>
                                <div class="col-md-4">
                                    <b-input-group class="float-right">
                                        <b-form-input v-model="search.keyword"></b-form-input>
                                        <b-input-group-append>
                                            <b-button class="btn-search" @click="getAllData()" variant="outline-success">Tìm
                                                kiếm</b-button>
                                        </b-input-group-append>
                                    </b-input-group>
                                </div>
                                <div class="col-md-2">
                                    <button class="btn btn-success btn-fill float-right"
                                        style="color: #fff; margin-right: 30px;" @click="showModalVenue(0)">
                                        Thêm mới
                                    </button>
                                </div>
                            </div>
                        </template>
                        <div class="row">
                            <div class="col-md-10">

                            </div>
                            <div class="col-md-2">
                                <label for="per-page-select" style="text-align: end;">Số bản ghi</label>
                                <b-form-select style="width:50% right:0" id="per-page-select" v-model="pageSize"
                                    @change="getAllData()" :options="pageSizeOption" size="sm"></b-form-select>
                            </div>
                        </div>
                        <b-table :current-page="pageNumber" id="table-venue" striped hover :fields="fields" :items="venues">
                            <template #cell(id)="data">
                                {{ data.value }}
                            </template>
                            <template #cell(name)="data">
                                {{ data.value }}
                            </template>
                            <template #cell(address)="data">
                                {{ data.value }}
                            </template>
                            <template #cell(capacity)="data">
                                {{ data.value }} người
                            </template>
                            <template #cell(action)="data">
                                <div style="font-size: 1.2rem; !important">
                                    <b-button class="table-btn" variant="danger" title="Xóa">
                                        <b-icon icon="trash">
                                        </b-icon>
                                    </b-button>
                                    <b-button @click="showModalVenue(data.item.id)" class="table-btn" variant="secondary"
                                        title="Xem chi tiết">
                                        <b-icon icon="pencil-square">
                                        </b-icon>
                                    </b-button>
                                </div>
                            </template>
                        </b-table>
                        <b-pagination v-model="pageNumber" :total-rows="totals" :per-page="pageSize"
                            aria-controls="table-venue"></b-pagination>
                    </card>
                    <b-modal id="modal-add-edit" ref="modal-add-edit" size="lg" title="Thêm sân vận động">
                        <form>
                            <div class="row">
                                <div class="col-md-6">
                                    <base-input type="text" label="Tên sân vận động" placeholder="Nhập tên sân vận động"
                                        v-model="updateVenue.name">
                                    </base-input>
                                    <base-input type="text" label="Địa chỉ" placeholder="Nhập địa chỉ"
                                        v-model="updateVenue.address">
                                    </base-input>
                                    <base-input type="number" label="Sức chứa" placeholder="Sức chứa"
                                        v-model="updateVenue.capacity">
                                    </base-input>
                                    <label for="">Mô tả</label>
                                    <textarea type="text" class="form-control border-input"
                                        v-model="updateVenue.description">
                                    </textarea>
                                </div>
                                <div class="col-md-6">
                                    <label for="image">Ảnh sân vận động</label>
                                    <br>
                                    <img v-if="updateVenue.image" style="max-width: 780px;" slot="image"
                                        :src="$fileUrl + updateVenue.image" alt="..." />
                                    <br>
                                    <div style="display: flex; margin: 10px;">
                                        <div style="margin: auto;">
                                            <form>
                                                <label v-if="updateVenue.image === null" for="">Tải lên ảnh sự
                                                    kiện</label>
                                                <br>
                                                <input @change="handleFileChange" type="file" id="myFile" name="filename">
                                                <button type="button" @click="uploadImageVenue()"
                                                    class="btn btn-info btn-fill btn-img">
                                                    <i class="nc-icon nc-cloud-upload-94"></i>
                                                </button>
                                                <button type="button" class="btn btn-info btn-fill btn-img">
                                                    <i class="nc-icon nc-zoom-split"></i>
                                                </button>
                                            </form>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </form>
                        <template #modal-footer="{ ok, cancel }">
                            <div style="margin: auto; width: 30%;">
                                <b-button class="buttonModal" size="lg" variant="success" @click="addEditVenue()">
                                    OK
                                </b-button>
                                <b-button class="buttonModal" size="lg" variant="secondary" @click="cancel()">
                                    Cancel
                                </b-button>
                            </div>
                        </template>
                    </b-modal>
                </div>
            </div>
        </div>
    </div>
</template>
    
<script>
import LTable from 'src/components/Table.vue'
import Card from 'src/components/Cards/Card.vue'
import axios from 'axios'
export default {
    components: {
        LTable,
        Card
    },
    data() {
        return {
            search: {
                keyword: null,
                status: null
            },
            eventStatuses: [
                {
                    id: 1,
                    name: "Khởi tạo"
                },
                {
                    id: 2,
                    name: "Mở bán vé"
                },
                {
                    id: 3,
                    name: "Ngừng bán vé"
                },
                {
                    id: 4,
                    name: "Đang diễn ra"
                },
                {
                    id: 5,
                    name: "Đã hủy"
                },
            ],
            id: 1,
            pageSize: 100,
            pageNumber: 1,
            venues: [],
            totals:0,
            pageSizeOption:[5,10,25,50,100],
            imageUpload: null,
            fields: ['id',
                { key: 'name', label: 'Tên sân vận động' },
                { key: 'address', label: 'Địa chỉ' },
                { key: 'capacity', label: 'Sức chứa' },
                { key: 'action', label: 'Thao tác' },
            ],
            updateVenue: {
                id: 0,
                name: null,
                address: null,
                capacity: 0,
                description: null,
                image: null
            },
        };
    },
    methods: {
        async getVenue() {
            try {
                const res = await axios.get(
                    "myticket/api/venue/find",
                    {
                        params: {
                            pageSize: this.pageSize,
                            pageNumber: this.pageNumber,
                            startDate: this.startDate,
                            endDate: this.endDate,
                            name: this.search.keyword,
                        },
                    }
                )
                this.totals = res.data.data.totalItems
                return res.data.data.items;
            } catch (error) {
                console.error('API Error:', error);
                throw error;
            }
        },
        handleFileChange(event) {
            // Update the 'file' data property when the file input changes
            this.imageUpload = event.target.files[0];
        },
        async uploadImageVenue() {
            const formData = new FormData();
            formData.append('file', this.imageUpload);
            await axios.post('myticket/api/file/upload', formData).then(
                res => this.updateVenue.image = res.data.data
            ).catch(
                err => err
            )
        },
        async getAllData() {
            try {
                this.venues = await this.getVenue();
            } catch (error) {
                console.error('Error fetching data:', error);
            }
        },
        async getVenueById(id) {
            try {
                const res = await axios.get(
                    "myticket/api/venue/find-by-id",
                    {
                        params: {
                            id: id,
                        },
                    }
                )
                return res.data.data;
            } catch (error) {
                console.error('API Error:', error);
                throw error;
            }
        },
        notifyVue(type, message, verticalAlign, horizontalAlign, color) {
            this.$notifications.notify(
                {
                    message: `<span><b>${type}</b> - ${message}</span>`,
                    icon: 'nc-icon nc-app',
                    horizontalAlign: horizontalAlign,
                    verticalAlign: verticalAlign,
                    type: color
                })
        },
        async showModalVenue(id) {
            try {
                if (id !== 0) {
                    this.updateVenue = await this.getVenueById(id)
                } else {
                    this.updateVenue.name = null,
                        this.updateVenue.address = null,
                        this.updateVenue.id = 0,
                        this.updateVenue.capacity = null,
                        this.updateVenue.description = null,
                        this.updateVenue.image = null
                }
                console.log(this.updateVenue)
                // this.venues = await this.findAllVenue();
                this.$nextTick(() => {
                    // Using $nextTick to ensure the modal component is updated
                    this.$refs['modal-add-edit'].show();
                });
            } catch (error) {
                console.error("Error fetching ticket details:", error);
            }
        },
        async addEditVenue() {
            if (this.updateVenue.id !== 0) {
                const response = await axios.put('myticket/api/venue/update', this.updateVenue, {
                    headers: {
                        'Content-Type': 'application/json'
                    },
                })
                if (response.data.code === 200) {
                    this.notifyVue('Thành công', 'Cập nhật sân vận động thành công', 'top', 'right', 'success')
                }
                else {
                    this.notifyVue('Thất bại', 'Cập nhật sân vận động thất bại', 'top', 'right', 'danger')
                }
            } else {
                const response = await axios.post('myticket/api/venue/create', {
                    name: this.updateVenue.name,
                    address: this.updateVenue.address,
                    capacity: this.updateVenue.capacity,
                    description: this.updateVenue.description,
                    image: this.updateVenue.image
                }, {
                    headers: {
                        'Content-Type': 'application/json'
                    },
                })
                if (response.data.code === 200) {
                    this.notifyVue('Thành công', 'Cập nhật sân vận động thành công', 'top', 'right', 'success')
                }
                else {
                    this.notifyVue('Thất bại', 'Cập nhật sân vận động thất bại', 'top', 'right', 'danger')
                }
            }
            this.$refs['modal-add-edit'].hide();
            this.getAllData()
        },
    },
    mounted() {
        this.getAllData();
    },
    computed: {
    },
    watch: {
        // Fetch data when the pageNumber changes
        pageNumber(newPage, oldPage) {
            if (newPage !== oldPage) {
                this.getAllData();
            }
        },
    },
};
</script>
    
<style>
.table-btn.btn {
    border: none !important;
}
</style>
    