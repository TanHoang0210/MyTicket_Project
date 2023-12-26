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
                                <div class="col-md-2">
                                    <b-form-select v-model="search.status" @change="getAllData()">
                                        <b-form-select-option :value="null">Tất cả</b-form-select-option>
                                        <b-form-select-option v-for="item in orderStatuses" :value="item.id">{{ item.name
                                        }}</b-form-select-option>
                                    </b-form-select>
                                </div>
                            </div>
                        </template>
                        <b-table striped hover :fields="fields" :items="orders">
                            <template #cell(status)="data">
                                <td style="color: blue;font-weight: 600;" v-if="data.item.status === 1">Khởi tạo</td>
                                <td style="color: greenyellow;font-weight: 600;" v-if="data.item.status === 2">Chưa thanh
                                    toán</td>
                                <td style="color: yellow;font-weight: 600;" v-if="data.item.status === 3">Đang thanh toán
                                </td>
                                <td style="color: #888;font-weight: 600;" v-if="data.item.status === 4">Đã hủy</td>
                                <td style="color: orange;font-weight: 600;" v-if="data.item.status === 5">Đã thanh toán</td>
                                <td style="color: green;font-weight: 600;" v-if="data.item.status === 6">Đã thanh toán</td>
                            </template>
                            <template #cell(action)="data">
                                <div style="font-size: 1.2rem; !important">
                                    <b-button @click="showModalOrder(data.item.id)" class="table-btn" variant="secondary"
                                        title="Xem chi tiết">
                                        <b-icon icon="pencil-square">
                                        </b-icon>
                                    </b-button>
                                </div>
                            </template>
                        </b-table>
                    </card>
                </div>
            </div>
        </div>
        <b-modal scrollable  id="modal-add-edit" size="lg" ref="modal-add-edit" title="Thông tin vé">
            <form>
                <div class="row">
                    <div class="col-md-12">
                        <base-input type="text" label="Tên Sự kiện" :disabled="true"
                            v-model="order.eventName">
                        </base-input>
                    </div>
                    <div class="col-md-6">
                        <base-input type="text" label="Mã đặt vé" 
                            v-model="order.orderCode">
                        </base-input>
                        <base-input type="text" label="Ngày đặt vé" 
                            v-model="order.orderDate">
                        </base-input>
                        <base-input type="text" label="Ngày diễn ra" 
                            v-model="order.organizationDay">
                        </base-input>
                        <base-input type="text" label="Sân vận động" 
                            v-model="order.venueName">
                        </base-input>
                        <base-input type="text" label="Địa chỉ" 
                            v-model="order.venueAddress">
                        </base-input>
                    </div>
                    <div class="col-md-6">
                        <base-input type="text" label="Hạng vé" 
                            v-model="order.ticketEventName">
                        </base-input>
                        <base-input type="text" label="Mã vé" 
                            v-model="order.ticketCode">
                        </base-input>
                        <base-input type="text" label="Mã chỗ ngồi" 
                            v-model="order.seatCode">
                        </base-input>
                        <base-input type="text" label="Giá" 
                            v-model="order.price">
                        </base-input>
                        <label for="status">Trạng thái</label>
                        <br>
                        <span style="color: blue;font-weight: 600;" v-if="order.status === 1">Khởi tạo</span>
                    <span style="color: greenyellow;font-weight: 600;" v-if="order.status === 2">Chưa thanh
                        toán</span>
                    <span style="color: yellow;font-weight: 600;" v-if="order.status === 3">Đang thanh toán
                    </span>
                    <span style="color: #888;font-weight: 600;" v-if="order.status === 4">Đã hủy</span>
                    <span style="color: orange;font-weight: 600;" v-if="order.status === 5">Đã thanh toán</span>
                    <span style="color: green;font-weight: 600;" v-if="order.status === 6">Đã thanh toán</span>
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
            fields: ['id',
                { key: 'orderCode', label: 'Mã đơn đặt vé ' },
                { key: 'orderDate', label: 'Ngày đặt vé' },
                { key: 'eventName', label: 'Tên sự kiện' },
                { key: 'ticketEventName', label: 'Hạng vé' },
                { key: 'status', label: 'Trạng thái' },
                { key: 'action', label: 'Thao tác' },
            ],
            orderStatuses: [
                {
                    id: 1,
                    name: "Khởi tạo"
                },
                {
                    id: 2,
                    name: "Chưa thanh toán"
                },
                {
                    id: 3,
                    name: "Đang thanh toán"
                },
                {
                    id: 4,
                    name: "Đã hủy"
                },
                {
                    id: 5,
                    name: "Đã thanh toán"
                },
                {
                    id: 6,
                    name: "Đã nhận vé"
                },
            ],
            order: {
                id: 0,
                orderId: 0,
                orderCode: "string",
                orderDate: "2023-12-24T07:45:53.588Z",
                eventDetailId: 0,
                eventName: "string",
                organizationDay: "2023-12-24T07:45:53.588Z",
                venueName: "string",
                venueAddress: "string",
                ticketId: 0,
                ticketEventName: "string",
                ticketCode: "string",
                seatCode: "string",
                price: 0,
                qrCode: "string",
                isExchange: true,
                status: 0
            }
        };
    },
    methods: {
        async getOrder() {
            try {
                const res = await axios.get(
                    "myticket/api/order/admin/order/find-all",
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
        async getOrderById(id) {
            try {
                const res = await axios.get(
                    "myticket/api/order/find-by-id",
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
        async showModalOrder(id) {
            console.log(id)
            try {
                    this.order = await this.getOrderById(id)
                    this.order.organizationDay = helpService.formatDate(this.order.organizationDay)
                    this.order.orderDate = helpService.formatDate(this.order.orderDate)
                    this.order.price = helpService.formatCurrency(this.order.price)
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
        keyword: function () {
            // Derived property based on 'search'
            return this.search.keyword;
        }
    },
    watch: {
        search: function (newVal) {
            // Custom logic when 'search' changes
            this.search.keyword = newVal;
        }
    }
};
</script>
    
<style>
.table-btn.btn {
    border: none !important;
}
</style>
    