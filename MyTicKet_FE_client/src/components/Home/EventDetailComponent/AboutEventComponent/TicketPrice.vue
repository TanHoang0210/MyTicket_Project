<template>
    <section id="ticketPrice" class="box-shadow py-4 pb-5 section-content bg-transparent">
        <div class="container">
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12 mgt-24">
                    <h1 class="black-heading">
                        Ticket Pricing
                    </h1>
                    <div class="prices-content" style="display: flex; flex-direction: row; flex-wrap: wrap;">
                        <div style="width: 50%; text-align: center;" v-for="(detail, index) in currentEvent.eventDetails">
                            <p>
                                <strong>
                                    <span style="color: var(--primary-color-hover-bold);">
                                        THÔNG TIN MỞ BÁN VÉ Ngày {{ index + 1 }}
                                    </span>
                                    <div v-if="currentUser !== null && detail.status !== 5" style="display: flex;">
                                        <button @click="showTicketTransferEvent(detail.id)"
                                            class="btn btn-outline-primary w-20 mb-3 action-btn viewmap-btn">
                                            Danh sách vé đươc chuyển nhượng</button>
                                    </div>
                                    <br>
                                    <strong>
                                        BẮT ĐẦU MỞ BÁN :
                                    </strong>
                                    <span>
                                        {{ formatDate(detail.startSaleTicketDate) }}
                                    </span>
                                    <br>
                                    <strong>
                                        ĐÓNG CỔNG BÁN VÉ :
                                    </strong>
                                    <span>
                                        {{ formatDate(detail.endSaleTicketDate) }}
                                    </span>
                                </strong>
                                <br>
                                Chỉ có tại
                                <router-link to="/" style="text-decoration: none; color: #000fff;">
                                    MyTicket.com
                                </router-link>
                            </p>
                            <hr>
                            <p>
                                <strong>
                                    GIÁ VÉ TIÊU CHUẨN
                                    <span>&nbsp;</span>
                                </strong>
                            </p>
                            <p>
                                <span>
                                    <span v-for="ticket in detail.ticketEvents">
                                        {{ ticket.name }} :
                                        <span style="color: red;">
                                            {{ formatCurrency(ticket.price) }}
                                        </span>
                                        <br>
                                    </span>
                                    <br>
                                    <i>*Giá vé trên đã bao gồm tất cả các dịch vụ đi kèm.*</i>
                                </span>

                            </p>
                            <hr>
                            <strong>
                                GHI CHÚ
                            </strong>
                            <ul style="list-style: none;">
                                <li>Số vé tối đa 1 người có thể mua là 10 vé</li>
                                <li>
                                    <strong>
                                        Nội quy mua vé tại My Ticket
                                    </strong>
                                    <ul style="list-style: none;">
                                        <li>
                                            Bạn sẽ có 10 phút để lựa chọn và mua vé
                                        </li>
                                        <li>
                                            Chuẩn bị sẵn sàng phương thức thanh toán bằng VNPay nhé!
                                        </li>
                                    </ul>
                                </li>
                            </ul>

                        </div>
                    </div>
                    <b-modal size="xl" ref="transfer-list-ticket" centered title="Danh sách vé đang được chuyển nhượng">
                        <div style="display: flex;">
                            <div class="transfer-ticket-card" v-for=" ticket in listTransferTicket">
                                <div class="transfer-ticket-card-inner">
                                    <h5><strong>Hạng vé: </strong><span style="float: right;">{{
                                        ticket.ticketName }}</span></h5>
                                    <h5><strong>Giá: </strong> <span style="float: right; ">{{
                                        formatCurrency(ticket.price) }}</span></h5>
                                    <h5><strong>Mã Chỗ: </strong> <span style="float: right;">{{
                                        ticket.seatCode }}</span></h5>
                                    <button @click="gotoPay(ticket.id, ticket.customerTransferId)" class="btn btn-fill w-20
                                        viewmap-btn">
                                        Thanh toán ngay
                                    </button>
                                </div>
                            </div>
                        </div>
                        <template #modal-footer="{ ok, cancel }">
                            <b-button size="xl" variant="secondary" @click="cancel()">
                                Cancel
                            </b-button>
                        </template>
                    </b-modal>
                </div>
            </div>
        </div>
    </section>
</template>
<script>
import moment from 'moment';
import numeral from 'numeral';
import axios from 'axios';
export default {
    props: ['currentEvent', 'currentUser'],
    data() {
        return {
            currentUser: null,
            pageSize: 5,
            pageNumber: 1,
            listTransferTicket: [
                {
                    id: null,
                    eventDetailId: null,
                    ticketName: null,
                    price: null,
                    seatCode: null,
                    customerTransferId: null
                }
            ]
        }
    },
    mounted() {
        this.currentUser = this.$store.getters.currentUser
    },
    methods: {
        formatDate(date) {
            // Chuyển đổi ngày thành định dạng dd/mm/yyyy
            return moment(date).format(' hh:mm DD/MM/YYYY');
        },
        formatCurrency(value) {
            return numeral(value).format('0,0') + ' VND';
        },
        async showTicketTransferEvent(eventDetailId) {
            try {
                this.listTransferTicket = await this.getListTransferTicket(eventDetailId)
                this.$nextTick(() => {
                    // Using $nextTick to ensure the modal component is updated
                    this.$refs['transfer-list-ticket'].show();
                });
            } catch (error) {
                console.error("Error fetching ticket details:", error);
            }
        },
        async getListTransferTicket(eventDetailId) {
            try {
                const res = await axios.get(
                    "myticket/api/ticket/find-all-transfer",
                    {
                        params: {
                            pageSize: this.pageSize,
                            pageNumber: this.pageNumber,
                            eventDetailId: eventDetailId
                        },
                    }
                )
                return res.data.data.items;
            } catch (error) {
                console.error('API 1 Error:', error);
                throw error;
            }
        },
        async gotoPay(id, customerId) {
            try {
                const res = await axios.post('api/Vnpay/transfer/payment-vn-pay', { ticketId: id }, {
                    headers: {
                        'Content-Type': 'application/json',
                    }
                })
                await axios.put('myticket/api/order/transfer/update-status',
                    {
                        ticketId: id,
                        transferStatus: 4,
                        customerTransferOwnerId: customerId
                    })
                this.payUrl = res.data;
                window.location.href = this.payUrl
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
                });
            }
        },
    }
}
</script>

<style>
.prices-content {
    font-size: 18px;
    letter-spacing: 1px;
}

.transfer-ticket-card {
    width: 25%;
    display: flex;
}

.transfer-ticket-card-inner {
    width: 90%;
    margin: auto;
    border: 2px dotted var(--primary-color-bold);
    border-radius: 5px 5px;
    padding: 5px 5px;
}
</style>