<template>
    <div>
        <div>
            <h2 class="black-heading">Vé Chuyển Nhượng</h2>
            <div class="alert alert-notify-purple">
                Tất cả vé mà bạn đã yêu cầu chuyển nhượng sẽ được hiển thị ở đây!
                <br>
                Bạn có thể hủy yêu cầu chuyển nhượng vé nếu vé chưa được chuyển nhượng nhé!
            </div>
            <div class="row">
                <div class="col-md-10">

                </div>
                <div class="col-md-2">
                    <b-form-select style="width:50% right:0" id="per-page-select" v-model="pageSize" @change="fetchData()"
                        :options="pageSizeOption" size="sm"></b-form-select>
                </div>
            </div>
            <b-table :current-page="pageNumber" id="table-ticket" class="custom-table" striped hover :fields="orderFields"
                :items="tickets">
                <template #cell(id)="data">
                    {{ data.index + 1 }}
                </template>
                <template #cell(transferDate)="data">
                    {{ formatDate(data.item.transferDate) }}
                </template>
                <template #cell(transferStatus)="data">
                    <div v-if="data.item.eventStatus !== 5">
                        <span style="color: blue;font-weight: 600;" v-if="data.item.transferStatus === 1">Khởi tạo</span>
                        <span style="color: orange;font-weight: 600;"
                            v-if="data.item.transferStatus === 2 || data.item.transferStatus === 4">Đang Xử lý</span>
                        <span style="color: greenyellow;font-weight: 600;" v-if="data.item.transferStatus === 5">Chuyển
                            nhượng thành công</span>
                        <span style="color: green;font-weight: 600;" v-if="data.item.transferStatus === 6">Đã hoàn
                            tiền</span>
                    </div>
                    <span v-else style="color: red;font-weight: 600;">Hủy Sự Kiện</span>
                </template>
                <template v-slot:cell(action)="data">
                    <b-dropdown variant="none" no-caret v-if="data.item.transferStatus !== 3">
                        <template #button-content>
                            <b-icon icon="three-dots"></b-icon>
                        </template>
                        <b-dropdown-item v-if="data.item.transferStatus === 1"
                            @click="showConfirmToTranferModal(data.item.id)">
                            Xác nhận
                        </b-dropdown-item>
                        <b-dropdown-item v-if="data.item.transferStatus !== 5"
                            @click="showCancelTranferModal(data.item.id)">
                            Hủy Yêu cầu
                        </b-dropdown-item>
                        <b-dropdown-item @click="showTicketDetail(data.item.id)">Xem chi tiết</b-dropdown-item>
                    </b-dropdown>
                </template>
            </b-table>
            <b-pagination v-model="pageNumber" :total-rows="totals" :per-page="pageSize"
                aria-controls="table-ticket"></b-pagination>
        </div>
        <b-modal size="lg" centered v-if="currentTicket !== null" ref="modal-myticket" hide-footer
            title="Thông tin chuyển nhượng">
            <div class="d-block text-center">
                <div style="display: flex;">
                    <div style="width: 100%;">
                        <div style="display: flex; flex-direction: row; justify-content: space-between;">
                            <h4>Ngày yêu cầu: {{ formatDate(currentTicket.transferDate) }}</h4>
                            <h4 v-if="currentTicket.transferStatus === 1">Trạng thái chuyển nhượng: <span
                                    style="color: blue;">Khởi tạo</span></h4>
                            <h4 v-if="currentTicket.transferStatus === 2">Trạng thái chuyển nhượng: <span
                                    style="color: orange;">Đang xử lý</span></h4>
                            <h4 v-if="currentTicket.transferStatus === 5">Trạng thái chuyển nhượng: <span
                                    style="color: #888;">Đã chuyển nhượng</span></h4>
                        </div>
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
                </div>
            </div>
        </b-modal>
        <div>
            <b-modal v-if="currentTicket !== null" ref="modal-transfer" centered title="Xác nhận hủy chuyển nhượng vé">
                <h4 class="my-4">Bạn có chắc chắn muốn hủy chuyển nhượng vé đi xem {{ currentTicket.eventName }} không?</h4>
                <template #modal-footer="{ ok, cancel }">
                    <b-button size="lg" variant="success" @click="confirmCancelTransferTicket(currentTicket.id)">
                        OK
                    </b-button>
                    <b-button size="lg" variant="secondary" @click="cancel()">
                        Cancel
                    </b-button>
                </template>
            </b-modal>
            <b-modal v-if="currentTicket !== null" ref="modal-confirm" centered title="Nhập mã xác nhận chuyển nhượng vé">
                <h4 class="my-4">Vui lòng nhập mã chuyển nhượng vé đã được gửi tới email của bạn để xác nhận việc chuyển
                    nhượng!</h4>
                <b-form-input v-model="confirmCode" type="text"></b-form-input>
                <template #modal-footer="{ ok, cancel }">
                    <b-button size="lg" variant="success" @click="confirmTransferCode(currentTicket.id)">
                        OK
                    </b-button>
                    <b-button size="lg" variant="secondary" @click="cancel()">
                        Cancel
                    </b-button>
                </template>
            </b-modal>
        </div>
    </div>
</template>
<script>
import axios from 'axios'
import moment from 'moment'
import numeral from 'numeral'
import store from '@/store'
import { TransferStatus } from '@/store/const'
export default {
    data() {
        return {
            statusTransfer: "",
            noData: false,
            pageSize: 5,
            pageNumber: 1,
            confirmCode: "",
            pageSizeOption: [5, 10, 25, 50],
            totals: 0,
            tickets: null,
            currentTicket: null,
            orderFields: [
                { key: 'id', label: 'ID' },
                { key: 'eventName', label: 'Tên sự kiện' },
                { key: 'transferDate', label: 'Ngày Yêu cầu' },
                { key: 'ticketEventName', label: 'Hạng vé' },
                { key: 'transferStatus', label: 'Trạng thái' },
                { key: 'action', label: 'Thao tác' },
            ]
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
        async showCancelTranferModal(id) {
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
                    "myticket/api/order/transfer/find-by-id",
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
        async confirmTransferCode(id) {
            const res = await axios.put('myticket/api/order/confirm-transfer',
                {
                    id: id,
                    confirmCode: this.confirmCode
                },
                {
                    headers: {
                        'Content-Type': 'application/json',
                    }
                })
            if (res.data.code === 200) {
                this.$refs['modal-confirm'].hide(),
                    this.$router.push('/order')
                this.$toasted.success("Xác nhận yêu cầu chuyển nhượng vé thành công", {
                    position: 'top-center',
                    duration: 3000, // Thời gian hiển thị toast (ms)
                    theme: 'outline', // Theme: 'outline', 'bubble'
                    iconPack: 'fontawesome', // Icon pack: 'fontawesome', 'mdi'
                    icon: 'time', // Tên icon, ví dụ: 'check' (cho fontawesome)
                    iconColor: 'white', // Màu của icon
                    containerClass: 'custom-toast-container-class', // Thêm class cho container
                    singleton: true, // Hiển thị toast duy nhất, không hiển thị toast mới nếu toast trước chưa biến mất
                })
                this.fetchData()
            } else {
                this.$toasted.error(res.data.message, {
                    position: 'top-center',
                    duration: 3000, // Thời gian hiển thị toast (ms)
                    theme: 'outline', // Theme: 'outline', 'bubble'
                    iconPack: 'fontawesome', // Icon pack: 'fontawesome', 'mdi'
                    icon: 'time', // Tên icon, ví dụ: 'check' (cho fontawesome)
                    iconColor: 'white', // Màu của icon
                    containerClass: 'custom-toast-container-class', // Thêm class cho container
                    singleton: true, // Hiển thị toast duy nhất, không hiển thị toast mới nếu toast trước chưa biến mất

                })
            }
        },
        async showConfirmToTranferModal(id) {
            try {
                this.currentTicket = await this.getOrderTicketDetail(id)
                this.$nextTick(() => {
                    // Using $nextTick to ensure the modal component is updated
                    this.$refs['modal-confirm'].show();
                });
            } catch (error) {
                console.error("Error fetching ticket details:", error);
            }
        },
        async confirmCancelTransferTicket(id) {
            try {
                await axios.put('myticket/api/order/cancel-tranfer',
                    {
                        orderDetailId: id
                    },
                    {
                        headers: {
                            'Content-Type': 'application/json',
                        }
                    })
                this.$refs['modal-transfer'].hide(),
                    this.$router.push('/order')
                this.$toasted.success("Hủy chuyển nhượng vé thành công", {
                    position: 'top-center',
                    duration: 3000, // Thời gian hiển thị toast (ms)
                    theme: 'outline', // Theme: 'outline', 'bubble'
                    iconPack: 'fontawesome', // Icon pack: 'fontawesome', 'mdi'
                    icon: 'time', // Tên icon, ví dụ: 'check' (cho fontawesome)
                    iconColor: 'white', // Màu của icon
                    containerClass: 'custom-toast-container-class', // Thêm class cho container
                    singleton: true, // Hiển thị toast duy nhất, không hiển thị toast mới nếu toast trước chưa biến mất
                })
            } catch (error) {
                this.$toasted.error(error.response.data.error_description, {
                    position: 'top-center',
                    duration: 3000, // Thời gian hiển thị toast (ms)
                    theme: 'outline', // Theme: 'outline', 'bubble'
                    iconPack: 'fontawesome', // Icon pack: 'fontawesome', 'mdi'
                    icon: 'time', // Tên icon, ví dụ: 'check' (cho fontawesome)
                    iconColor: 'white', // Màu của icon
                    containerClass: 'custom-toast-container-class', // Thêm class cho container
                    singleton: true, // Hiển thị toast duy nhất, không hiển thị toast mới nếu toast trước chưa biến mất

                }),
                    this.$refs['modal-transfer'].hide()
            }
            this.fetchData()
        },
        async getMyOrderInfo() {
            console.log(store.state.accessToken)
            try {
                const res = await axios.get(
                    "myticket/api/order/transfer/find-all",
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
                    this.tickets.forEach(element => {
                        if (element.transferStatus === 1) {
                            this.statusTransfer = 1
                        } else if (element.transferStatus === 2) {
                            this.statusTransfer = 2
                        }
                        else if (element.transferStatus === 3) {
                            this.statusTransfer = 3
                        }
                    });
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
        this.handleStatus()
    },
    watch: {
        // Fetch data when the pageNumber changes
        pageNumber(newPage, oldPage) {
            if (newPage !== oldPage) {
                this.fetchData();
            }
        },
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