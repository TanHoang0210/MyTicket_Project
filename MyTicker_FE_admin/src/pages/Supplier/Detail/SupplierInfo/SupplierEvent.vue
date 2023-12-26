<template>
    <div class="content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-12">
                    <card class="strpied-tabled-with-hover" body-classes="table-full-width table-responsive">
                        <template slot="header">
                            <div class="row" style="display: flex; justify-content: space-between;">
                                <div class="col-md-6">
                                    <h4 class="card-title">Danh sách vé khách hàng</h4>
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
                            </div>
                        </template>
                        <b-table striped hover :fields="fields" :items="orders">
                            <template #cell(exchangeStatus)="data">
                                <td style="color: blue;font-weight: 600;" v-if="data.item.exchangeStatus === 1">Khởi tạo</td>
                                <td style="color:orange;font-weight: 600;" v-if="data.item.exchangeStatus === 2">Đã xác nhận</td>
                                <td style="color: #888;font-weight: 600;" v-if="data.item.exchangeStatus === 3">Đã hủy
                                </td>
                                <td style="color: green;font-weight: 600;" v-if="data.item.exchangeStatus === 4">Trả vé thành công</td>
                            </template>
                            <template #cell(action)="data">
                                <div style="font-size: 1.2rem; !important">
                                    <b-button @click="showModalExchange(data.item.id)" class="table-btn" variant="secondary"
                                        title="Xem chi tiết">
                                        <b-icon icon="pencil-square">
                                        </b-icon>
                                    </b-button>
                                </div>
                            </template>
                        </b-table>
                    </card>
                    <b-modal id="modal-add-edit" scrollable ref="modal-add-edit" size="lg" title="Thông tin trả vé" ok-only>
                        <form>
                            <div class="row">
                                <div class="col-md-12">
                                    <base-input type="text" label="Tên Sự kiện" :disabled="true"
                                        v-model="exchange.eventName">
                                    </base-input>
                                </div>
                                <div class="col-md-6">
                                    <base-input type="text" label="Mã đặt vé" 
                                        v-model="exchange.orderCode">
                                    </base-input>
                                    <base-input type="text" label="Ngày đặt vé" 
                                        v-model="exchange.orderDate">
                                    </base-input>
                                    <base-input type="text" label="Ngày diễn ra" 
                                        v-model="exchange.organizationDay">
                                    </base-input>
                                    <base-input type="text" label="Sân vận động" 
                                        v-model="exchange.venueName">
                                    </base-input>
                                    <base-input type="text" label="Địa chỉ" 
                                        v-model="exchange.venueAddress">
                                    </base-input>
                                    <base-input type="text" label="Ngày yêu cầu trả vé" 
                                        v-model="exchange.exchangeDate">
                                    </base-input>
                                </div>
                                <div class="col-md-6">
                                    <base-input type="text" label="Hạng vé" 
                                        v-model="exchange.ticketEventName">
                                    </base-input>
                                    <base-input type="text" label="Mã vé" 
                                        v-model="exchange.ticketCode">
                                    </base-input>
                                    <base-input type="text" label="Mã chỗ ngồi" 
                                        v-model="exchange.seatCode">
                                    </base-input>
                                    <base-input type="text" label="Giá" 
                                        v-model="exchange.price">
                                    </base-input>
                                    <base-input v-if="exchange.exchangeStatus === 3" type="text" label="Ngày hủy trả vé" 
                                        v-model="exchange.exchangeCancelDate">
                                    </base-input>
                                    <base-input v-if="exchange.exchangeStatus === 5" type="text" label="Ngày hoàn trả" 
                                        v-model="exchange.exchangeDoneDate">
                                    </base-input>
                                    <label for="status">Trạng thái</label>
                                    <br>
                                    <span style="color: blue;font-weight: 600;" v-if="exchange.exchangeStatus === 1">Khởi tạo
                                </span>
                                <span style="color: green;font-weight: 600;" v-if="exchange.exchangeStatus === 2">Đã xác nhận
                                </span>
                                <span style="color: #888;font-weight: 600;" v-if="exchange.exchangeStatus === 3">Đã hủy</span>
                                <span style="color: yellow;font-weight: 600;" v-if="exchange.exchangeStatus === 4">Trả vé thành công</span>
                                </div>
                            </div>
                        </form>
                        <template #modal-footer="{ ok, cancel }">
                            <div style="margin: auto; width: 100%;">
                                <b-button class="buttonModal" size="lg" variant="secondary" @click="cancel()">
                                    Đóng
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
import helpService from 'src/service/help/helpService'
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
            isUpdate: false,
            pageSize: 100,
            pageNumber: 1,
            orders: [],
            exchange: {
                id: 0,
                orderId: 0,
                orderCode: "string",
                orderDate: "2023-12-24T17:54:45.118Z",
                eventDetailId: 0,
                eventName: "string",
                organizationDay: "2023-12-24T17:54:45.118Z",
                venueName: "string",
                venueAddress: "string",
                ticketId: 0,
                ticketEventName: "string",
                ticketCode: "string",
                seatCode: "string",
                price: 0,
                exchangeStatus: 0,
                exchangeDate: "2023-12-24T17:54:45.118Z",
                exchangeDoneDate: "2023-12-24T17:54:45.118Z",
                exchangeCancelDate: "2023-12-24T17:54:45.118Z"
            },
            imageUpload: null,
            fields: ['id',
                { key: 'orderCode', label: 'Mã đơn đặt vé ' },
                { key: 'orderDate', label: 'Ngày đặt vé' },
                { key: 'eventName', label: 'Tên sự kiện' },
                { key: 'ticketEventName', label: 'Hạng vé' },
                { key: 'exchangeStatus', label: 'Trạng thái trả vé' },
                { key: 'action', label: 'Thao tác' },
            ]
        };
    },
    methods: {
        async getOrder() {
            try {
                const res = await axios.get(
                    "myticket/api/order/admin/exchange/find-all",
                    {
                        params: {
                            customerId: this.$route.query.id,
                            pageSize: this.pageSize,
                            pageNumber: this.pageNumber,
                            keyword: this.search.keyword,
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
        async getAllData() {
            try {
                this.orders = await this.getOrder();
                this.orders.forEach(element => {
                    element.orderDate = helpService.formatDate(element.orderDate)
                });
            } catch (error) {
                console.error('Error fetching data:', error);
            }
        },
        async getExchangeById(id) {
            try {
                const res = await axios.get(
                    "myticket/api/order/exchange/find-by-id",
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
        async showModalExchange(id) {
            console.log(id)
            try {
                this.exchange = await this.getExchangeById(id)
                this.exchange.exchangeCancelDate = helpService.formatDate(this.exchange.exchangeCancelDate)
                this.exchange.exchangeDate = helpService.formatDate(this.exchange.exchangeDate)
                this.exchange.exchangeDoneDate = helpService.formatDate(this.exchange.exchangeDoneDate)
                this.exchange.orderDate = helpService.formatDate(this.exchange.orderDate)
                this.exchange.organizationDay = helpService.formatDate(this.exchange.organizationDay)
                this.exchange.price = helpService.formatCurrency(this.exchange.price)
                // this.venues = await this.findAllVenue();
                this.$nextTick(() => {
                    // Using $nextTick to ensure the modal component is updated
                    this.$refs['modal-add-edit'].show();
                });
            } catch (error) {
                console.error("Error fetching ticket details:", error);
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
    