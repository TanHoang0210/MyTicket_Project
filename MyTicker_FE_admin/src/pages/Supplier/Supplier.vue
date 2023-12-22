<template>
    <div class="content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-12">
                    <card class="strpied-tabled-with-hover" body-classes="table-full-width table-responsive">
                        <template slot="header">
                            <div class="row" style="display: flex; justify-content: space-between;">
                                <div class="col-md-6">
                                    <h4 class="card-title">Danh sách nhà cung cấp</h4>
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
                                        style="color: #fff; margin-right: 30px;" @click="showModalType(0)">
                                        Thêm mới
                                    </button>
                                </div>
                            </div>
                        </template>
                        <b-table striped hover :fields="fields" :items="eventTypes">
                            <template #cell(id)="data">
                                {{ data.value }}
                            </template>
                            <template #cell(name)="data">
                                {{ data.value }}
                            </template>
                            <template #cell(description)="data">
                                {{ data.value }}
                            </template>
                            <template #cell(eventTypeImage)="data">
                                <div style="display: flex;">
                                    <img style="max-width: 200px; margin: auto;" slot="image" :src="$fileUrl + data.value"
                                        alt="..." />
                                </div>
                            </template>
                            <template #cell(action)="data">
                                <div style="font-size: 1.2rem; !important">
                                    <b-button class="table-btn" variant="danger" title="Xóa">
                                        <b-icon icon="trash">
                                        </b-icon>
                                    </b-button>
                                    <b-button @click="showModalType(data.item.id)" class="table-btn" variant="secondary"
                                        title="Xem chi tiết">
                                        <b-icon icon="pencil-square">
                                        </b-icon>
                                    </b-button>
                                </div>
                            </template>
                        </b-table>
                    </card>
                    <b-modal id="modal-add-edit" ref="modal-add-edit" size="lg" title="Thêm sân vận động">
                        <form>
                            <div class="row">
                                <div class="col-md-6">
                                    <base-input type="text" label="Tên sân vận động" placeholder="Nhập tên sân vận động"
                                        v-model="updateEventType.name">
                                    </base-input>
                                    <label for="">Mô tả</label>
                                    <textarea type="text" class="form-control border-input"
                                        v-model="updateEventType.description">
                                    </textarea>
                                </div>
                                <div class="col-md-6">
                                    <label for="image">Ảnh sự kiện</label>
                                    <br>
                                    <img v-if="updateEventType.eventTypeImage" style="max-width: 100%;" slot="image"
                                        :src="$fileUrl + updateEventType.eventTypeImage" alt="..." />
                                    <br>
                                    <div style="display: flex; margin: 10px;">
                                        <div style="margin: auto;">
                                            <form>
                                                <label v-if="updateEventType.eventTypeImage === null" for="">Tải lên ảnh sự
                                                    kiện</label>
                                                <br>
                                                <input @change="handleFileChange" type="file" id="myFile" name="filename">
                                                <button type="button" @click="uploadImageType()"
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
                                <b-button class="buttonModal" size="lg" variant="success" @click="addEditType()">
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
            id: 0,
            pageSize: 100,
            pageNumber: 1,
            eventTypes: [],
            imageUpload: null,
            fields: ['id',
                { key: 'fullName', label: 'Tên đầy đủ doanh nghiệp ' },
                { key: 'shortName', label: 'Tên doanh nghiệp' },
                { key: 'address', label: 'Địa chỉ' },
                { key: 'taxCode', label: 'Mã số ' },
                { key: 'action', label: 'Thao tác' },
            ],
            updateEventType: {
                id: 0,
                name: null,
                description: null,
                image: null
            },
        };
    },
    methods: {
        async getEventType() {
            try {
                const res = await axios.get(
                    "myticket/api/user/supplier/find-all",
                    {
                        params: {
                            pageSize: this.pageSize,
                            pageNumber: this.pageNumber,
                            startDate: this.startDate,
                            endDate: this.endDate,
                            keyword: this.search.keyword,
                            eventTypeId: this.$route.query.eventTypeId,
                            status: this.search.status
                        },
                    }
                )
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
        async uploadImageType() {
            const formData = new FormData();
            formData.append('file', this.imageUpload);
            await axios.post('myticket/api/file/upload', formData).then(
                res => this.updateEventType.image = res.data.data
            ).catch(
                err => err
            )
            console.log(this.updateEventType.image)
        },
        async getAllData() {
            try {
                this.eventTypes = await this.getEventType();
            } catch (error) {
                console.error('Error fetching data:', error);
            }
        },
        async getEventTypeById(id) {
            try {
                const res = await axios.get(
                    "myticket/api/event-type/find-by-id",
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
        async showModalType(id) {
            console.log(id)
            try {
                if (id !== 0) {
                    this.updateEventType = await this.getEventTypeById(id)
                }
                // this.venues = await this.findAllVenue();
                this.$nextTick(() => {
                    // Using $nextTick to ensure the modal component is updated
                    this.$refs['modal-add-edit'].show();
                });
            } catch (error) {
                console.error("Error fetching ticket details:", error);
            }
        },
        async addEditType() {
            try {
                if (this.updateEventType.id !== 0) {
                }
            } catch (error) {

            }
        },
    },
    mounted() {
        this.getAllData();
    },
    computed: {
    }
};
</script>
    
<style>
.table-btn.btn {
    border: none !important;
}
</style>
    