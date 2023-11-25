<template>
    <div>
        <div>
            <table class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12">
                <thead class="tb_head">
                    <tr>
                        <th class=" tb_col col_1">ID</th>
                        <th class=" tb_col col_2">Mã Đặt Vé</th>
                        <th class=" tb_col col_4">Thông Tin Sự kiện</th>
                        <th class=" tb_col col_4">Thông Tin Vé</th>
                        <th class=" tb_col col_1">Thao Tác</th>
                    </tr>
                </thead>
                <tbody v-if="noData">
                    <tr>
                        <td colspan="4" class="text-center">Giỏ vé của bạn hiện đang trống</td>
                    </tr>
                </tbody>
                <tbody v-else>
                    <tr v-for="(item,index) in tickets">
                        <td class="">{{ index+1}}</td>
                        <td class="">{{ item.orderCode }}</td>
                        <td>
                            <tr>
                                <td>Ngày Đặt vé: </td>
                                <td class="text-center">{{ formatDate(item.orderDate) }}</td>
                            </tr>
                            <tr>
                                <td>Tên sự kiện: </td>
                                <td class="text-center">{{ item.eventName }}</td>
                            </tr>
                            <tr>
                                <td>Thời gian: </td>
                                <td class="text-center">{{ formatDate(item.organizationDay) }}</td>
                            </tr>
                            <tr>
                                <td>Địa điểm:</td>
                                <td class="text-center">{{ item.venueName }} - {{ item.venueAddress }}</td>
                            </tr>
                        </td>
                        <td>
                            <tr>
                                <td>Hạng vé:</td>
                                <td>{{ item.ticketEventName }}</td>
                                <td rowspan="3">
                                    <img class="" style="width:150px;"   :src="$fileUrl+'api/file/get?folder=test&file=testqr.webp'" alt="">
                                </td>
                            </tr>
                            <tr>
                                <td>Thông tin vé: </td>
                                <td>{{ item.ticketCode }} - {{ item.seatCode }}</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Giá:</td>
                                <td>{{ formatCurrency(item.price) }}</td>
                                <td></td>
                            </tr>
                            
                        </td>
                        <td class="">
                            <div class="ticket-action ">
                                <b-button variant="info">Chuyển Nhượng Vé</b-button>
                                <b-button variant="warning">Trả Vé</b-button>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
            <b-pagination v-model="currentPage" :total-rows="totalRows" :per-page="perPage"
                aria-controls="my-table"></b-pagination>
        </div>
    </div>
</template>
<script>
import axios from 'axios'
import moment from 'moment'
import numeral from 'numeral'
import store from '@/store'
export default {
    data() {
        return {
            noData: false,
            pageSize: 10,
            pageNumber: 1,
            tickets: [
                {
                    id: 11,
                    orderId: 2,
                    orderCode: "",
                    orderDate: "",
                    eventDetailId: 1,
                    eventName: "Sự kiện tổ chức",
                    organizationDay: "2023-11-25T07:27:57.066",
                    venueName: "Sân vận động Nhà Bè",
                    venueAddress: "12 Nhà Bè",
                    ticketId: 43,
                    ticketEventName: "CAT 3",
                    ticketCode: "CWT9R",
                    seatCode: "SEAT3",
                    price: 1000000,
                    qrCode: null
                }
            ]
        }
    },
    methods: {
        async getMyOrderInfo() {
            console.log(store.state.accessToken)
            try {
                const res = await axios.get(
                    "/myticket/api/order/find-all",
                    {
                        params: {
                            pageSize: this.pageSize,
                            pageNumber: this.pageNumber,
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
                this.tickets = await this.getMyOrderInfo();
                if (this.tickets.length === 0) {
                    this.noData = true
                } else {
                    this.noData = false
                }
            } catch (error) {
                this.$toasted.error(error.message, {
                    position: 'top-right',
                    duration: 3000, // Thời gian hiển thị toast (ms)
                });
            }
        },
        formatDate(date) {
            // Chuyển đổi ngày thành định dạng dd/mm/yyyy
            return moment(date).format('hh:mm DD/MM/YYYY');
        },
        formatCurrency(value) {
            return numeral(value).format('0,0') + ' VND';
        },
    },
    mounted() {
        this.fetchData()
    },
}
</script>
<style>
.ticket-info {
    display: flex;
    justify-content: space-between
}

.ticket-action {
    display: flex;
    flex-direction: column;
}

table {
    border-collapse: collapse;
    width: 100%;
    border: 1px solid #ddd;
    /* Màu và độ dày của border cho bảng */
}

th,
td {
    border: 1px solid #ddd;
    border-top: none;
}

.tb_head {
    background-color: #f2f2f2;
    /* Màu nền cho phần head của bảng */
}

.tb_col {
    font-weight: bold;
    /* In đậm cho các ô header */
}</style>