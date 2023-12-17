<template>
    <div class="content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <card>
                        <div class="row">
                            <h4 slot="header" class="card-title col-md-6">Nhập thông tin tổng quan sự kiện</h4>
                            <div class="text-center col-md-6">
                                <button type="submit" class="btn btn-success btn-fill float-right "
                                    @click.prevent="createEvent">
                                    Tiếp Tục
                                </button>
                            </div>
                        </div>
                        <form>
                            <div class="row">
                                <div class="col-md-12">
                                    <base-input type="text" label="Tên sự kiện" placeholder="Nhập tên sự kiện"
                                        v-model="eventInfo.eventName">
                                    </base-input>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6" style="display: flex;">
                                    <div>
                                        <label for="eventTypeId">Loại sự kiện</label>
                                        <br>
                                        <select class="select-form" v-model="eventInfo.eventTypeId" label="eventTypeId"
                                            name="eventTypeId" id="eventTypeId">
                                            <option value="0" disabled>Chọn loại sự kiện</option>
                                            <option v-for="item in listType" :value="item.id">{{ item.name }}</option>
                                        </select>
                                    </div>
                                    <div style="margin-left: 30px;">
                                        <br>
                                        <button type="submit" class="btn btn-success btn-fill" @click.prevent="createEvent">
                                            Thêm loại sự kiện
                                        </button>
                                    </div>
                                </div>
                                <div class="col-md-6" style="display: flex;">
                                    <div>
                                        <label for="supplierId">Nhà cung cấp</label>
                                        <br>
                                        <select class="select-form" v-model="eventInfo.supplierId" label="supplierId"
                                            name="supplierId" id="supplierId">
                                            <option value="0" disabled>Chọn nhà cung cấp</option>
                                            <option v-for="item in listSupplier" :value="item.id">{{ item.fullName }} - {{
                                                item.shortName }}</option>
                                        </select>
                                    </div>
                                    <div style="margin-left: 30px;">
                                        <br>
                                        <button type="submit" class="btn btn-success btn-fill" @click.prevent="createEvent">
                                            Thêm nhà cung cấp
                                        </button>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Mô tả sự kiện</label>
                                        <textarea rows="10" class="form-control border-input"
                                            placeholder="Here can be your description" v-model="eventInfo.eventDescription">
                                    </textarea>
                                    </div>
                                </div>
                                <div class="col-md-8">
                                    <label for="image">Ảnh sự kiện</label>
                                    <br>
                                    <img style="max-width: 780px;" slot="image" :src="$fileUrl + eventInfo.eventImage"
                                        alt="..." />
                                    <br>
                                    <div style="display: flex; margin: 10px;">
                                        <div style="margin: auto;">
                                            <form action="/">
                                                <label v-if="eventInfo.eventImage === null" for="">Tải lên ảnh sự kiện</label>
                                                <br>
                                                <input type="file" id="myFile" name="filename">
                                                <button type="submit" class="btn btn-info btn-fill btn-img">
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
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label>Đuy định đổi trả</label>
                                        <textarea rows="5" class="form-control border-input"
                                            placeholder="Here can be your description" v-model="eventInfo.exchangePolicy">
                                        </textarea>
                                    </div>
                                    <div class="form-group">
                                        <label>Nội quy sự kiện</label>
                                        <textarea rows="5" class="form-control border-input"
                                            placeholder="Here can be your description" v-model="eventInfo.admissionPolicy">
                                        </textarea>
                                    </div>
                                </div>
                            </div>
                        </form>
                    </card>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
import Card from 'src/components/Cards/Card.vue'
import axios from 'axios'
export default {
    components: {
        Card
    },
    data() {
        return {
            listType: [
                {
                    id: 0,
                    name: "string",
                    description: "string",
                    eventTypeImage: "string"
                }
            ],
            listSupplier: [
                {
                    id: 0,
                    fullName: "string",
                    shortName: "string",
                    address: "string",
                    taxCode: "string"
                }
            ],
            eventInfo: {
                eventName: "",
                eventTypeId: 0,
                supplierId: 0,
                eventDescription: "",
                exchangePolicy: "",
                admissionPolicy: "",
                eventImage: null,
                isExchange: true,
            }
        }
    },
    methods: {
        createEvent() {
            alert('Your data: ' + JSON.stringify(this.eventInfo))
        },
        async getCategories() {
            try {
                const res = await axios.get(
                    "myticket/api/event-type/find",
                    {
                        params: {
                            pageSize: 10,
                            pageNumber: 1
                        },
                    }
                )
                return res.data.data.items;
            } catch (error) {
                console.error('API 1 Error:', error);
                throw error;
            }
        },
        async getSupplier() {
            try {
                const res = await axios.get(
                    "myticket/api/user/supplier/find-all",
                    {
                        params: {
                            pageSize: 10,
                            pageNumber: 1
                        },
                    }
                )
                return res.data.data.items;
            } catch (error) {
                console.error('API 1 Error:', error);
                throw error;
            }
        },
        async fetchData() {
            try {
                this.listType = await this.getCategories();
                this.listSupplier = await this.getSupplier();
            } catch (error) {
                console.error('API 1 Error:', error);
                throw error;
            }
        },
    }, mounted() {
        this.fetchData();
        console.log(this.listType)
    },


}

</script>
<style>
.select-form {
    width: 100%;
    height: 40px;
    border: 1px solid #E3E3E3;
    border-radius: 4px;
    padding: 8px 12px;
    color: #565656;
    font-size: 1rem;
}

.btn-img {
    margin: 0 5px;
}
</style>
  