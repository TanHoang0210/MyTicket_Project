<template>
    <div>
        <div>
            <h2 class="black-heading">Vé Của Tôi</h2>
            <div class="alert alert-notify-purple">
                Tất cả vé mà bạn đã mua thành công sẽ được hiển thị ở đây!
                <br>
                Bạn có quyền chuyển nhượng vé cho người khách hoặc hoàn trả vé (với sự đồng ý từ bên thứ 3).
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
                <template #cell(organizationDay)="data">
                    {{ formatDate(data.item.organizationDay) }}
                </template>
                <template #cell(status)="data">
                    <div v-if="data.item.eventStatus !== 5">
                        <span style="color: blue;font-weight: 600;" v-if="data.item.status === 1">Khởi tạo</span>
                        <span style="color: greenyellow;font-weight: 600;" v-if="data.item.status === 2">Chưa thanh
                            toán</span>
                        <span style="color: yellow;font-weight: 600;" v-if="data.item.status === 3">Đang thanh toán
                        </span>
                        <span style="color: #888;font-weight: 600;" v-if="data.item.status === 4">Đã hủy</span>
                        <span style="color: orange;font-weight: 600;" v-if="data.item.status === 5">Đã thanh toán</span>
                        <span style="color: green;font-weight: 600;" v-if="data.item.status === 6">Đã thanh toán</span>
                        <span style="color: chocolate;font-weight: 600;" v-if="data.item.status === 10">Mua Lại</span>
                    </div>
                    <span v-else style="color: red;font-weight: 600;">Hủy Sự Kiện</span>
                </template>
                <template v-slot:cell(action)="data">
                    <b-dropdown variant="none" no-caret>
                        <template #button-content>
                            <b-icon icon="three-dots"></b-icon>
                        </template>
                        <b-dropdown-item v-if="data.item.eventStatus !== 5 && data.item.status !== 10"
                            @click="showTranferModal(data.item.id)">
                            Chuyển nhượng vé
                        </b-dropdown-item>
                        <b-dropdown-item
                            v-if="data.item.eventStatus !== 5 && data.item.status !== 10 && data.item.isExchange == true"
                            @click="showExchangeModal(data.item.id)">
                            Trả vé
                        </b-dropdown-item>
                        <b-dropdown-item @click="showTicketDetail(data.item.id)">
                            Xem chi tiết
                        </b-dropdown-item>
                    </b-dropdown>
                </template>
            </b-table>
            <b-pagination v-model="pageNumber" :total-rows="totals" :per-page="pageSize"
                aria-controls="table-ticket"></b-pagination>
        </div>
        <div>
            <b-modal v-if="currentTicket !== null" ref="modal-transfer" centered title="Xác nhận chuyển nhượng vé">
                <h4 class="my-4">Bạn có chắc chắn muốn chuyển nhượng lại vé đi xem {{ currentTicket.eventName }} không?</h4>
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
                    <b-button size="lg" variant="success" @click="confirmExchange(currentTicket.id)">
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
            pageSize: 10,
            pageNumber: 1,
            total: 0,
            tickets: null,
            currentTicket: null,
            pageSizeOption: [5, 10, 25, 50],
            totals: 0,
            orderFields: [
                { key: 'id', label: 'ID' },
                { key: 'eventName', label: 'Tên sự kiện' },
                { key: 'organizationDay', label: 'Ngày diễn ra' },
                { key: 'ticketEventName', label: 'Hạng vé' },
                { key: 'status', label: 'Trạng thái' },
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
                    this.$refs['modal-transfer'].hide(),
                    this.$router.push('/transfer'),
                    this.$toasted.success("Yêu cầu chuyển nhượng vé thành công", {
                        position: 'top-center',
                        duration: 3000, // Thời gian hiển thị toast (ms)
                        theme: 'outline', // Theme: 'outline', 'bubble'
                        iconPack: 'fontawesome', // Icon pack: 'fontawesome', 'mdi'
                        icon: 'time', // Tên icon, ví dụ: 'check' (cho fontawesome)
                        iconColor: 'white', // Màu của icon
                        containerClass: 'custom-toast-container-class', // Thêm class cho container
                        singleton: true, // Hiển thị toast duy nhất, không hiển thị toast mới nếu toast trước chưa biến mất
                    })
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

                    }),
                    this.$refs['modal-transfer'].hide()
                )
            this.fetchData()
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
        async confirmExchange(id) {
            const res = await axios.put('myticket/api/order/exchange-ticker',
                {
                    orderDetailId: id
                },
                {
                    headers: {
                        'Content-Type': 'application/json',
                    }
                })
            if (res.data.code === 200) {
                this.$refs['modal-exchange'].hide(),
                    this.$router.push('/refund'),
                    this.$toasted.success("Yêu cầu trả vé thành công", {
                        position: 'top-center',
                        duration: 3000, // Thời gian hiển thị toast (ms)
                        theme: 'outline', // Theme: 'outline', 'bubble'
                        iconPack: 'fontawesome', // Icon pack: 'fontawesome', 'mdi'
                        icon: 'time', // Tên icon, ví dụ: 'check' (cho fontawesome)
                        iconColor: 'white', // Màu của icon
                        containerClass: 'custom-toast-container-class', // Thêm class cho container
                        singleton: true // Hiển thị toast duy nhất, không hiển thị toast mới nếu toast trước chưa biến mất
                    })
                this.fetchData()

            } else {
                this.$toasted.error(err.response.data.error_description, {
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
                if (res.data.code === 200) {
                    return res.data.data;
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
                if (res.data.code === 200) {
                    this.totals = res.data.data.totalItems
                    return res.data.data.items;
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
            } catch (error) {
                console.error('API 1 Error:', error);
                throw error;
            }
        },
        async fetchData() {
            this.tickets = await this.getMyOrderInfo();
            if (this.tickets.length === 0) {
                this.noData = true
            } else {
                this.noData = false
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


.btn-primary {
    margin: 0 !important;
}

.qrCode {
    border: 2px solid #ccc;
    width: 250px;
}

.custom-table th {
    background-color: var(--primary-color-bold);
    /* Set your desired background color */
    color: #ffffff;
    /* Set your desired text color */
    border-color: var(--primary-color-bold);
    /* Set your desired border color */
}

.title-ticket {
    margin-left: 10px;
}


.btn-myticket {
    margin: auto;
    width: 90%;
    margin-bottom: 5px;
}
</style>