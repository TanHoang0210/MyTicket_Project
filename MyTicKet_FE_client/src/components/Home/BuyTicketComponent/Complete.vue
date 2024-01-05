<template>
    <div>
        <div v-if="isLoading">
            <h2 v-html="expression">
            </h2>
        </div>
        <div v-else style="display: flex;">
            <!-- Biểu thức hoàn thành sẽ có class "completed" -->
            <div v-if="isComplete" class="completeInfo" :class="{ completed: isComplete }">
                <!-- Nội dung của biểu thức -->
                <img class="circle-border" style="width: 30%; margin: auto;"
                    src="https://yourimageshare.com/ib/F0UmAVYWKI.webp" alt="">
                <h2 v-html="expression">
                </h2>
                <b-button @click="letgo()" style="width: 20%; margin: auto;" variant="success">Nhận Vé</b-button>
            </div>
            <div v-else class="completeInfo" :class="{ completed: isComplete }">
                <!-- Nội dung của biểu thức -->
                <img class="circle-border" style="width: 30%; margin: auto;"
                    src="https://yourimageshare.com/ib/M5o1YCoLcG.webp" alt="">
                <h2 v-html="expression">
                </h2>
                <b-button @click="letgo()" style="width: 20%; margin: auto;" variant="secondary">Về trang chủ</b-button>
            </div>
        </div>
    </div>
</template>
<script>
import LoadPage from "@/views/LoadPage.vue"
import Header from "@/components/Header.vue";
import { BIcon } from 'bootstrap-vue';
import axios from "axios";
export default {
    name: 'BuyTicketView',
    components: {
        LoadPage, Header
    },
    data() {
        return {
            isLoading: false,
            isComplete: false,
            drawingInProgress: false,
            borderWidth: 0,
            expression: "",
        }
    },
    mounted() {
        this.isLoading = true;
        this.expression = "MyTicket đang tiến hành đặt vé vui lòng chờ trong giây lát!"
        this.checkOrder()
    },
    methods: {
        async checkOrder() {
            const params = this.$route.query;
            if (params.vnp_ResponseCode === '00' && params.vnp_TransactionStatus === '00') {
                // Giao dịch thành công, bạn có thể xử lý dữ liệu khác từ params nếu cần thiết
                await this.completedOrder(params.vnp_OrderInfo,params.vnp_TxnRef,params.vnp_PayDate)
                this.isComplete = true;
                this.expression = 'Đơn hàng của bạn đã hoàn thành!<br>Vào mục vé của bạn để xem thông tin vé đã đặt nhé!'
            } else {
                await this.canceldOrder(params.vnp_OrderInfo)
                // Giao dịch thất bại hoặc hủy bỏ
                this.isComplete = false;
                this.expression = 'OOPS!<br>Đơn hàng của bạn không thể hoàn thành vui lòng đặt lại vé!'

            }
            this.isLoading = false;
        },
        letgo() {
            if (!this.isComplete) {
                this.$router.push('/')
            } else {
                this.$router.push('/order')
            }

        },
        async completedOrder(orderId,txnRef,transDate) {
            try {
                const res = await axios.put('myticket/api/order/update-order-status',
                    {
                        id: orderId,
                        status: 5,
                        transactionNo:txnRef,
                        transDate:transDate
                    })
            } catch (error) {
                this.$toasted.error('Oops! Đã xảy ra lỗi! Vui lòng thử lại', {
                    position: 'top-right',
                    duration: 3000, // Thời gian hiển thị toast (ms)
                });
            }
        },
        async canceldOrder(orderId) {
            try {
                const res = await axios.put('myticket/api/order/update-order-status',
                    {
                        id: orderId,
                        status: 4
                    })
            } catch (error) {
                this.$toasted.error('Oops! Đã xảy ra lỗi! Vui lòng thử lại', {
                    position: 'top-right',
                    duration: 3000, // Thời gian hiển thị toast (ms)
                });
            }
        }

    }
}
</script>
<style>
.completed {
    /* Màu của biểu thức hoàn thành */
    animation: fadeIn 1s ease-out;
    /* Tên, thời gian và kiểu chuyển động animation */
}

@keyframes fadeIn {
    from {
        opacity: 0;
        /* Bắt đầu với độ mờ */
    }

    to {
        opacity: 1;
        /* Kết thúc với độ rõ nét */
    }
}

.completeInfo {
    margin: auto;
    display: flex;
    flex-direction: column;
    text-align: center;
}

.circle-border {
    transition: all 0.5s ease;
    /* Hiệu ứng chuyển động của border */
}</style>