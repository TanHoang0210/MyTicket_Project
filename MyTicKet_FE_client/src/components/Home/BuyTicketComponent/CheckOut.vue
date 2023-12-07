<template>
    <div>
        <div class="row">
            <div v-if="haveOrder" class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12">
                <div id="cancelTimeCountdown" class="alert alert-danger h4"
                    style="display: flex; justify-content: space-around; font-size: 1.2rem; letter-spacing: 1px;">
                    <span>
                        Chúng tôi sẽ giữ vé giúp bạn trong vòng 5 phút!
                        Kiểm tra lại thông tin vé đã đặt nhé!
                        <br>
                        Sau 5 phút bạn chưa tiến hành thanh toán chúng tôi sẽ tự động hủy vé của bạn
                    </span>
                    <span class="h1">
                        {{ formatTime(countdownTime) }}
                    </span>
                </div>
                <div class="user-info-order">
                    <div class="info-order">
                        <div class="info-order-inner">
                            <h3 class="info-content">Thông Tin Khách Hàng</h3>
                            <div class="">
                                <div class="row mgb-8">
                                    <div class="col-lg-3 col-md-2 col-sm-3 col-xs-12 col-12">Họ tên</div>
                                    <div class="col-lg-9 col-md-10 col-sm-9 col-xs-12 col-12"><strong>
                                            {{ currentUser.firstName }} {{ currentUser.lastName }}</strong></div>
                                </div>
                                <div class="row mgb-8">
                                    <div class="col-lg-3 col-md-2 col-sm-3 col-xs-12 col-12">Email</div>
                                    <div class="col-lg-9 col-md-10 col-sm-9 col-xs-12 col-12">
                                        <strong>{{ currentUser.email }} </strong>
                                    </div>
                                </div>
                                <div class="row mgb-8">
                                    <div class="col-lg-3 col-md-2 col-sm-3 col-xs-12 col-12">Số điện thoại</div>
                                    <div class="col-lg-9 col-md-10 col-sm-9 col-xs-12 col-12"><strong>{{ currentUser.phone
                                    }}</strong>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="info-order">
                        <div class="info-order-inner">
                            <h3 class="info-content">Phương thức thanh toán</h3>
                            <div class="">
                                <div class="mgt-12" id="individualPayment-checkoutform-paymentid-88">
                                    <div style="display: flex; flex-direction: column;"
                                        class=" field-checkoutform-paymentid-88">

                                        <label>
                                            <input type="radio" id="checkoutform-paymentid-1" name="CheckoutForm[paymentId]"
                                                value="1" checked="" readonly="readonly"> VNPay (Thanh toán qua cổng trực
                                            tuyến
                                            của VNPay)</label>
                                        <label>
                                            <input type="radio" id="checkoutform-paymentid-2" name="CheckoutForm[paymentId]"
                                                value="2" checked="" readonly="readonly"> Momo (Quét mã nhanh chóng, tận
                                            hưởng
                                            nhiều ưu đãi)</label>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="ticker-info-order">
                    <h3 style="text-align: center;" class="info-content no-border padding-top-30">Thông Tin Đơn Đặt Vé</h3>
                    <h6 style="text-align: center; margin-bottom: 10px;">Mã Đơn Đặt: <strong>{{ order.orderCode }}</strong>
                    </h6>
                    <div id="shoppingCartBox">
                        <table id="cartList" class="table table-bordered">
                            <thead>
                                <tr>
                                    <th>
                                        <span class="hex_Order_number visible-desktop ">
                                            Chi Tiết Sự Kiện
                                        </span>
                                    </th>
                                    <th class="col-xs-2">Thông tin chỗ ngồi</th>
                                    <th class="col-xs-2">Thông tin vé</th>
                                    <th class="col-xs-1">Giá Tiền</th>
                                    <th class="col-xs-1">Thao tác</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="item in order.orderDetails" class="orderTicket 23_afasingapore-cb91ad25">
                                    <td>
                                        <div class="text-start event-info">
                                            <span class="text-bold ticketEventName">
                                                {{ item.eventName }} | {{ formatDate(item.organizationDay) }}</span>
                                            <br>
                                            <span class="ticketEventNote">
                                                <b-icon icon="geo-alt"></b-icon>
                                                {{ item.venueName }} - {{ item.venueAddress }}
                                                <br>

                                            </span>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="d-block d-md-none text-primary text-bold">Seat Info</div>
                                        <div class="area-name">{{ item.seatCode }} - {{ item.ticketCode }}</div>
                                    </td>
                                    <td>
                                        <div class="d-block d-md-none text-primary text-bold">Ticket Info</div>
                                        <div class="ticket-info">
                                            {{ item.ticketEventName }} </div>
                                    </td>
                                    <td class="text-nowrap"><span class="hidden-desktop  text-primary text-bold">
                                        </span>{{ formatCurrency(item.price) }}</td>
                                    <td class="initial">
                                        <button @click="deleteOrderDetail(item.id)"
                                            class="btn btn-danger btn-sm cancelTicket" title="" data-toggle="tooltip"
                                            data-placement="top" data-id="01325001032012" data-message="" onclick=""
                                            style="" data-bs-original-title="">Remove</button>
                                    </td>
                                </tr>
                                <tr style="background-color: var(--primary-color);">
                                    <td colspan="4" class="visible-desktop">
                                        <span style="float: right;">Thành Tiền</span>
                                    </td>
                                    <td class="text-center">
                                        <span id="orderAmount" class="text-danger">{{ formatCurrency(order.total) }}</span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12 text-center mgt-16">
                    <b-button @click="cancelOrder()" class="action-btn" variant="outline-secondary">Hủy Đơn Hàng</b-button>
                    <b-button class="action-btn" variant="outline-secondary">Tiếp Tục Đặt Vé</b-button>
                    <b-button @click="gotoPay()" class="action-btn" type="button" id="submitButton"
                        variant="outline-success">
                        <i class="fa fa-check"></i>
                        Checkout</b-button>
                </div>
            </div>
            <div v-else>
                Không có đơn hàng nào
            </div>
        </div>
    </div>
</template>
<script>
import store from '@/store'
import { OrderStatuses } from '@/store/const'
import axios from 'axios'
import moment from 'moment'
import numeral from 'numeral'
export default {
    data() {
        return {
            haveOrder: false,
            countdownTime: 0,
            order: {
                id: 1,
                orderCode: "5SAS5AS2",
                customerId: 1,
                status: 2,
                orderDate: "",
                qrCode: null,
                total: 0,
                orderDetails: [
                    {
                        id: 6,
                        orderId: 2,
                        eventDetailId: 1,
                        eventName: "",
                        organizationDay: "",
                        venueName: "",
                        ticketId: 1,
                        ticketEventName: "",
                        ticketCode: "",
                        seatCode: "",
                        price: 1000
                    }
                ]
            },
            payUrl: ''
        }
    },
    methods: {
        async getOrderInfo() {
            try {
                const res = await axios.get("myticket/api/order/find-order/for-pay")
                if (res.data.status == 1) {
                    this.haveOrder = true;
                }
                else {
                    this.haveOrder = false;
                    this.$router.push('/order')
                    this.$toasted.info('Hiện tại bạn không có đơn hàng nào', {
                        position: 'top-right',
                        duration: 3000, // Thời gian hiển thị toast (ms)
                    });
                }
                return res.data.data
            } catch (error) {
                console.error('API 1 Error:', error);
                throw error;
            }
        },
        async gotoPay() {
            try {
                const res = await axios.post('api/Vnpay/payment-vn-pay', { orderId: this.order.id },{
                    headers: {
                        'Content-Type': 'application/json',
                    }})
                this.payUrl = res.data;
                await this.payingOrder(this.order.id)
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
        async deleteOrderDetail(id) {
            try {
                await axios.delete(`myticket/api/order/ticket/delete/${id}`)
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
            this.fetchData()
        },
        async payingOrder(orderId) {
            try {
                const res = await axios.put('myticket/api/order/update-order-status',
                    {
                        id: orderId,
                        status: 3
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
                });
            }
        },
        async cancelOrder() {
            try {
                await axios.put('myticket/api/order/update-order-status',
                    {
                        id: this.order.id,
                        status: 4
                    })
                this.$toasted.info("Hủy đơn hàng thành công", {
                    position: 'top-right',
                    duration: 3000, // Thời gian hiển thị toast (ms)
                    theme: 'outline', // Theme: 'outline', 'bubble'
                    iconPack: 'fontawesome', // Icon pack: 'fontawesome', 'mdi'
                    icon: 'check', // Tên icon, ví dụ: 'check' (cho fontawesome)
                    iconColor: 'white', // Màu của icon
                    containerClass: 'custom-toast-container-class', // Thêm class cho container
                    singleton: true, // Hiển thị toast duy nhất, không hiển thị toast mới nếu toast trước chưa biến mất
                });
                clearInterval(this.countdownInterval);
                this.$router.push('/order')
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
        async fetchData() {
            try {
                this.order = await this.getOrderInfo();
                this.startCountdown();
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
        formatDate(date) {
            // Chuyển đổi ngày thành định dạng dd/mm/yyyy
            return moment(date).format('hh:mm DD/MM/YYYY');
        },
        formatCurrency(value) {
            return numeral(value).format('0,0') + ' VND';
        },
        formatTime(seconds) {
            const minutes = Math.floor(seconds / 60);
            const remainingSeconds = seconds % 60;
            return `${minutes}:${remainingSeconds < 10 ? '0' : ''}${remainingSeconds}`;
        },
        startCountdown() {
            // Kiểm tra xem có một đếm ngược khác đã bắt đầu chưa, nếu có thì hủy nó trước khi bắt đầu mới
            if (this.countdownInterval) {
                clearInterval(this.countdownInterval);
            }
            const orderDateInSeconds = (new Date(this.order.orderDate).getTime() / 1000) + 300; // Thêm 300 giây vào thời gian đặt hàng
            const currentSeconds = Math.floor(new Date().getTime() / 1000);
            this.countdownTime = Math.floor(orderDateInSeconds-currentSeconds, 0);
            // Thiết lập một interval để giảm giá trị của countdownTime mỗi giây
            this.countdownInterval = setInterval(() => {
                if (this.order.status != 3 || this.order.status != 4) {
                    if (this.countdownTime > 0) {
                        this.countdownTime--;
                        store.commit('setCoutDownOrder', this.countdownTime)
                    } else {
                        // Hủy đơn hàng khi hết thời gian
                        this.cancelOrder();
                    }
                }
            }, 1000);
        },
    },
    mounted() {
        this.fetchData();
        this.countdownTime = store.getters.countdownTime;
    },
    computed: {
        currentUser() {
            return this.$store.getters.currentUser;
        }
    },
    beforeDestroy() {
        clearInterval(this.countdownInterval);
    },
}
</script>
<style>
.user-info-order {
    margin-top: 50px;
    display: flex;
    justify-content: space-between;
}

.no-border {
    border-bottom: none !important;
}

.info-order {
    width: 49%;
    border: 1px solid var(--primary-color-bold);
    background-color: var(--primary-color);
}

.info-order-inner {
    margin-left: 20px;
    padding: 10px 0;
}

.padding-top-30 {
    padding-top: 30px;
}

.info-content {
    border-bottom: 2px solid;
    width: calc(100% - 20px);
    margin-bottom: 10px;
}

.action-btn {
    margin: 5px 5px;
}
</style>