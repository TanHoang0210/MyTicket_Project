<template>
    <div>
        <div>
            <h2 class="black-heading">Vé Của Tôi</h2>
            <div class="alert alert-notify-purple">
                Tất cả vé mà bạn đã mua thành công sẽ được hiển thị ở đây!
                <br>
                Bạn có quyền chuyển nhượng vé cho người khách hoặc hoàn trả vé (với sự đồng ý từ bên thứ 3).
            </div>
            <table class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12">
                <thead class="tb_head">
                    <tr>
                        <th class="text-center firstCol">#ID</th>
                        <th>Tên Sự Kiện</th>
                        <th>Thời Gian</th>
                        <th>Địa Điểm</th>
                        <th>Hạng Vé</th>
                        <th>Trạng Thái</th>
                        <th class="lastCol">Thao Tác</th>
                    </tr>
                </thead>
                <tbody v-if="noData">
                    <tr>
                        <td colspan="4" class="text-center">Giỏ vé của bạn hiện đang trống</td>
                    </tr>
                </tbody>
                <tbody v-else>
                    <tr v-for="(item, index) in tickets">
                        <td class="firstCol text-center ticket-infomation">{{ index + 1 }}</td>
                        <td class="ticket-infomation">
                            {{ item.eventName }}
                        </td>
                        <td class="ticket-infomation">
                            {{ formatDate(item.organizationDay) }}
                        </td>
                        <td class="ticket-infomation">
                            {{ item.venueName }} - {{ item.venueAddress }}
                        </td>
                        <td class="ticket-infomation">
                            {{ item.ticketEventName }}
                        </td>
                        <td class="ticket-infomation">
                            {{ item.ticketEventName }}
                        </td>
                        <td class="lastCol ticket-infomation">
                            <div class="ticket-action ">
                                <b-button id="showTransfer" @click="showTranferModal(item.id)" class="btn-myticket"
                                    variant="info">Chuyển
                                    Nhượng</b-button>
                                <b-button v-if="item.isExchange == true" id="showTransfer"
                                    @click="showExchangeModal(item.id)" class="btn-myticket" variant="warning">Trả
                                    Vé</b-button>
                                <b-button id="showDetail" @click="showTicketDetail(item.id)" class="btn-myticket"
                                    variant="secondary">Xem chi
                                    tiết</b-button>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div>
            <b-modal v-if="currentTicket !== null" ref="modal-transfer" centered title="Xác nhận chuyển nhượng vé">
                <h4 class="my-4">Bạn có chắc chắn muốn trả lại vé đi xem {{ currentTicket.eventName }} không?</h4>
                <template #modal-footer="{ ok, cancel }">
                    <b-button size="lg" variant="success" @click="confirmTransferTicket(currentTicket.id)">
                        OK
                    </b-button>
                    <b-button size="lg" variant="secondary" @click="cancel()">
                        Cancel
                    </b-button>
                </template>
            </b-modal>
        </div>
        <div>
            <b-modal v-if="currentTicket !== null" ref="modal-exchange" centered title="Xác nhận trả vé">
                <h4 class="my-4">Bạn có chắc chắn muốn trả lại vé đi xem {{ currentTicket.eventName }} không?</h4>
                <template #modal-footer="{ ok, cancel }">
                    <b-button size="lg" variant="success" @click="confirmExchange()">
                        OK
                    </b-button>
                    <b-button size="lg" variant="secondary" @click="cancel()">
                        Cancel
                    </b-button>
                </template>
            </b-modal>
        </div>
        <b-modal size="xl" centered v-if="currentTicket !== null" ref="modal-myticket" hide-footer
            title="Chi tiết vé của bạn">
            <div class="d-block text-center">
                <div style="display: flex;">
                    <div style="width: 80%;">
                        <table style="border: none; text-align: left; font-size: 1.2rem;">
                            <tr style="border: none;">
                                <td class="title-ticket">
                                    <span>
                                        Mã đặt vé
                                    </span>
                                    <span style="float: right;">
                                        |
                                    </span>
                                </td>
                                <td class="text-center">
                                    {{ currentTicket.orderCode }}
                                </td>
                            </tr>
                            <tr>
                                <td class="title-ticket">
                                    <span>
                                        Ngày đặt vé
                                    </span>
                                    <span style="float: right;">
                                        |
                                    </span>
                                </td>
                                <td class="text-center">
                                    {{ formatDate(currentTicket.orderDate) }}
                                </td>
                            </tr>
                            <tr>
                                <td class="title-ticket">
                                    <span>
                                        Tên sự kiện
                                    </span>
                                    <span style="float: right;">
                                        |
                                    </span>
                                </td>
                                <td class="text-center">
                                    {{ currentTicket.eventName }}
                                </td>
                            </tr>
                            <tr>
                                <td class="title-ticket">
                                    <span>
                                        Thời gian diễn ra
                                    </span>
                                    <span style="float: right;">
                                        |
                                    </span>
                                </td>
                                <td class="text-center">
                                    {{ formatDate(currentTicket.organizationDay) }}
                                </td>
                            </tr>
                            <tr>
                                <td class="title-ticket">
                                    <span>
                                        Địa Điểm
                                    </span>
                                    <span style="float: right;">
                                        |
                                    </span>
                                </td>
                                <td class="text-center">
                                    {{ currentTicket.venueName }} - {{ currentTicket.venueAddress }}
                                </td>
                            </tr>
                            <tr>
                                <td class="title-ticket">
                                    <span>
                                        Hạng vé
                                    </span>
                                    <span style="float: right;">
                                        |
                                    </span>
                                </td>
                                <td class="text-center">
                                    {{ currentTicket.ticketEventName }}
                                </td>
                            </tr>
                            <tr>
                                <td class="title-ticket">
                                    <span>
                                        Mã vé
                                    </span>
                                    <span style="float: right;">
                                        |
                                    </span>
                                </td>
                                <td class="text-center">
                                    {{ currentTicket.ticketCode }}
                                </td>
                            </tr>
                            <tr>
                                <td class="title-ticket">
                                    <span>
                                        Mã chỗ ngồi
                                    </span>
                                    <span style="float: right;">
                                        |
                                    </span>
                                </td>
                                <td class="text-center">
                                    {{ currentTicket.seatCode }}
                                </td>
                            </tr>
                            <tr>
                                <td class="title-ticket">
                                    <span>
                                        Giá vé
                                    </span>
                                    <span style="float: right;">
                                        |
                                    </span>
                                </td>
                                <td class="text-center">
                                    {{ formatCurrency(currentTicket.price) }}
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="display: flex; flex-direction: column;">
                        <div style="margin: auto;">
                            <span>
                                Bấm vào để lưu QR về máy
                            </span>
                            <img ref="myQR" @click="downloadImage" style="cursor: pointer;" class="qrCode"
                                :src="$fileUrl + currentTicket.qrCode" alt="">
                        </div>
                    </div>
                </div>
            </div>
        </b-modal>
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
            pageSize: 5,
            pageNumber: 1,
            total: 0,
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
            ],
            currentTicket: null
        }
    },
    methods: {
        downloadImage() {
            // Lấy đường dẫn đến ảnh từ thuộc tính src của thẻ img
            const imageUrl = this.$refs.myQR.src;
            const parts = imageUrl.split("&file=");
            const fileName = parts[parts.length - 1];
            // Tạo đối tượng Blob từ ảnh
            fetch(imageUrl)
                .then((response) => response.blob())
                .then((blob) => {
                    // Tạo đường link để tải xuống
                    const link = document.createElement("a");
                    link.href = window.URL.createObjectURL(blob);
                    link.download = fileName;

                    // Thêm link vào body
                    document.body.appendChild(link);

                    // Kích hoạt sự kiện click để tải xuống
                    link.click();

                    // Xóa link sau khi đã sử dụng
                    document.body.removeChild(link);
                });
        },
        async showTicketDetail(id) {
            try {
                this.currentTicket = await this.getOrderTicketDetail(id)
                this.$nextTick(() => {
                    // Using $nextTick to ensure the modal component is updated
                    this.$refs['modal-myticket'].show();
                });
            } catch (error) {
                console.error("Error fetching ticket details:", error);
            }
        },
        async confirmTransferTicket(id) {
                await axios.put('myticket/api/order/transfer-ticker',
                    {
                        orderDetailId: id
                    }, 
                    {
                    headers: {
                        'Content-Type': 'application/json',
                    }
                }).then(res => 
                    this.$refs['modal-transfer'].hide()
                ).catch(err => 
                this.$toasted.error(err.response.data.error_description, {
                    position: 'top-center',
                    duration: 3000, // Thời gian hiển thị toast (ms)
                    theme: 'outline', // Theme: 'outline', 'bubble'
                    iconPack: 'fontawesome', // Icon pack: 'fontawesome', 'mdi'
                    icon: 'time', // Tên icon, ví dụ: 'check' (cho fontawesome)
                    iconColor: 'white', // Màu của icon
                    containerClass: 'custom-toast-container-class', // Thêm class cho container
                    singleton: true, // Hiển thị toast duy nhất, không hiển thị toast mới nếu toast trước chưa biến mất
                }))
                this.$refs['modal-transfer'].hide()
        },
        async showExchangeModal(id) {
            try {
                this.currentTicket = await this.getOrderTicketDetail(id);
                this.$nextTick(() => {
                    // Using $nextTick to ensure the modal component is updated
                    this.$refs['modal-exchange'].show();
                });
            } catch (error) {
                console.error("Error fetching ticket details:", error);
            }
        },
        async showTranferModal(id) {
            try {
                this.currentTicket = await this.getOrderTicketDetail(id)
                this.$nextTick(() => {
                    // Using $nextTick to ensure the modal component is updated
                    this.$refs['modal-transfer'].show();
                });
            } catch (error) {
                console.error("Error fetching ticket details:", error);
            }
        },
        async getOrderTicketDetail(id) {
            try {
                const res = await axios.get(
                    "/myticket/api/order/find-by-id",
                    {
                        params: {
                            id: id
                        }
                    }
                )
                return res.data.data;
            } catch (error) {
                console.error('API 1 Error:', error);
                throw error;
            }
        },
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
                this.total = res.data.data.totalItems
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

.modal-header {
    background-color: var(--primary-color-bold) !important;
}

.modal-title {
    font-size: 1.2rem !important;
    color: #fff !important;
}

.ticket-action {
    display: flex;
    flex-direction: column;
}

table {
    text-align: center;
    border-collapse: collapse;
    width: 100%;
    /* Màu và độ dày của border cho bảng */
}

.btn-primary {
    margin: 0 !important;
}

.qrCode {
    border: 2px solid #ccc;
    width: 250px;
}

.title-ticket {
    margin-left: 10px;
}

th,
td {
    border-top: none;
}

.firstCol {
    border-right: 1px solid #ccc;
}

.lastCol {
    border-left: 1px solid #ccc;
}

th {
    font-size: 1.2rem;
    padding: 5px 10px;
}

.tb_head {
    background-color: #f2f2f2;
    /* Màu nền cho phần head của bảng */
}

.tb_col {
    font-weight: bold;
    /* In đậm cho các ô header */
}

.btn-myticket {
    margin: auto;
    width: 90%;
    margin-bottom: 5px;
}

tr,
td {
    border: none;
    border-bottom: 1px solid #ccc;
}

.ticket-infomation {}</style>