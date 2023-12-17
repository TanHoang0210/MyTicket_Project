<template>
    <div class="content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <card>
                        <div class="row">
                            <h4 slot="header" class="card-title col-md-6">Thông tin chi tiết sự kiện</h4>
                        </div>
                        <form>
                            <div class="row">
                                <div class="col-md-6">
                                    <label>Đuy định đổi trả</label>
                                    <textarea type="text" label="Tên sự kiện" class="form-control border-input"
                                        :disabled="true" placeholder="Light dashboard" v-model="eventInfo.eventName">
                                    </textarea>
                                </div>
                                <div class="col-md-3">
                                    <base-input type="text" label="Nhà cung cấp" :disabled="true"
                                        placeholder="Light dashboard" v-model="eventInfo.supllier">
                                    </base-input>
                                </div>
                                <div class="col-md-3">
                                    <label for="eventTypeId">Loại Sự Kiện</label>
                                    <br>
                                    <select class="select-form" v-model="eventInfo.eventTypeId" label="eventTypeId"
                                        name="eventTypeId" id="eventTypeId">
                                        <option v-for="item in listType" :value="item.id">{{ item.name }}</option>
                                    </select>
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
                                            <button class="btn btn-info btn-fill btn-img">
                                                <i class="nc-icon nc-cloud-download-93"></i>
                                            </button>
                                            <button class="btn btn-info btn-fill btn-img">
                                                <i class="nc-icon nc-zoom-split"></i>
                                            </button>
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
                                <div class="text-center col-md-6">
                                <button type="submit" class="btn btn-info btn-fill float-right "
                                    @click.prevent="updateProfile">
                                    Cập nhật
                                </button>
                            </div>
                            </div>
                        </form>
                    </card>
                    <h4 slot="header" class="card-title col-md-6">Danh sách các ngày diễn ra sự kiện</h4>
                    <card v-for="item in eventInfo.eventDetails">
                        <div class="row">
                            <div class="col-md-4">
                                <base-input type="text" label="Ngày diễn ra" :disabled="true" placeholder="Light dashboard"
                                    v-model="item.organizationDay">
                                </base-input>
                                <base-input type="text" label="Ngày mở bán vé" placeholder="Username"
                                    v-model="item.startSaleTicketDate">
                                </base-input>
                                <base-input type="text" label="Ngày đóng bán vé" placeholder="Username"
                                    v-model="item.endSaleTicketDate">
                                </base-input>
                                <base-input type="text" label="Sân vận động" placeholder="Username"
                                    v-model="item.venueName">
                                </base-input>
                                <base-input type="text" label="Địa điểm" placeholder="Username" v-model="item.venueAddress">
                                </base-input>
                                <card style="display: flex; margin: 10px;">
                                    <h4 slot="header" class="card-title">Danh sách các loại vé</h4>
                                    <table style="border: 1px solid #ccc; width: 100%;">
                                        <tbody>
                                            <tr v-for="ticket in item.ticketEvents">
                                                <td>{{ ticket.name }}</td>
                                                <td>{{ ticket.price }}</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </card>
                                <div class="text-center col-md-6">
                                <button type="submit" class="btn btn-info btn-fill float-right "
                                    @click.prevent="updateProfile">
                                    Cập nhật
                                </button>
                            </div>
                            </div>
                            <div class="col-md-8">
                                <label for="image">Ảnh Sơ đồ sự kiện</label>
                                <br>
                                <img style="max-width: 780px;" slot="image" :src="$fileUrl + item.eventSeatMapImage"
                                    alt="..." />
                                <br>
                                <div style="display: flex; margin: 10px;">
                                    <div style="margin: auto;">
                                        <button class="btn btn-info btn-fill btn-img">
                                            <i class="nc-icon nc-cloud-download-93"></i>
                                        </button>
                                        <button class="btn btn-info btn-fill btn-img">
                                            <i class="nc-icon nc-zoom-split"></i>
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </card>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
import Card from 'src/components/Cards/Card.vue'
import axios from 'axios'
import helpService from 'src/service/help/helpService'
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
            eventInfo: {
                id: 0,
                eventName: "string",
                eventTypeId: 0,
                eventTypeName: "1",
                eventDescription: "string",
                eventImage: "string",
                admissionPolicy: "string",
                exchangePolicy: "string",
                status: 0,
                firstEventDate: "2023-12-10T12:36:01.468Z",
                lastEventDate: "2023-12-10T12:36:01.468Z",
                supllier: "string",
                eventDetails: [
                    {
                        id: 0,
                        eventId: 0,
                        eventName: "string",
                        eventImage: "string",
                        venueId: 0,
                        venueName: "string",
                        organizationDay: "2023-12-10T12:36:01.468Z",
                        startSaleTicketDate: "2023-12-10T12:36:01.468Z",
                        endSaleTicketDate: "2023-12-10T12:36:01.468Z",
                        eventSeatMapImage: "string",
                        seatSelectType: 0,
                        havingSeatMap: true,
                        status: 0,
                        ticketEvents: [
                            {
                                id: 0,
                                name: "string",
                                price: 0,
                                eventDetailId: 0,
                                status: 0,
                                quantity: 0
                            }
                        ]
                    }
                ]
            }
        }
    },
    methods: {
        updateProfile() {
            alert('Your data: ' + JSON.stringify(this.eventInfo))
        },
        async getEventDetail() {
            try {
                const res = await axios.get(
                    "/myticket/api/event/find-by-id", {
                    params: {
                        id: this.$route.query.id
                    }
                })
                return res.data.data
            } catch (error) {
                console.error('API 1 Error:', error);
                throw error;
            }
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
        async fetchData() {
            try {
                this.eventInfo = await this.getEventDetail()
                this.listType = await this.getCategories();
                this.eventInfo.eventDetails.forEach(element => {
                    element.organizationDay = helpService.formatDate(element.organizationDay)
                    element.endSaleTicketDate = helpService.formatDate(element.endSaleTicketDate)
                    element.startSaleTicketDate = helpService.formatDate(element.startSaleTicketDate)
                    element.ticketEvents.forEach(e => {
                        e.price = helpService.formatCurrency(e.price)
                    });
                });
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
  