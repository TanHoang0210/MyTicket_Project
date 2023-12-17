<template>
    <div>
        <div v-if="isLoading">
        <LoadPage/>
    </div>
        <div v-else style="display: flex;">
            <!-- Biểu thức hoàn thành sẽ có class "completed" -->
            <div v-if="isComplete" class="completeInfo" :class="{ completed: isComplete }">
                <!-- Nội dung của biểu thức -->
                <b-icon class="circle-border" :style="{ 'border-width': borderWidth }" icon="check-lg"
                    :animation="throbAnimation" font-scale="8"></b-icon>
                <h2 v-html="expression">
                </h2>
                <b-button @click="letgo()" style="width: 20%; margin: auto;" variant="success">Nhận Vé</b-button>
            </div>
            <div v-else class="completeInfo" :class="{ completed: isComplete }">
                <!-- Nội dung của biểu thức -->
                <b-icon class="circle-border-red" :style="{ 'border-width': borderWidth }" icon="x-lg"
                    :animation="throbAnimation" font-scale="8"></b-icon>
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
            id:null,
            customerId:null,
            isLoading:false,
            isComplete: false,
            drawingInProgress: false,
            borderWidth: 0,
            throbAnimation: 'throb',
            expression: "",
        }
    },
    mounted() {
        this.isLoading = true;
        this.checkOrder()
        this.startDrawing()
        setTimeout(() => {
            this.throbAnimation = null;
        }, 800); // Đặt thời gian tùy thuộc vào thời gian hiệu ứng throb
    },
    methods: {
        startDrawing() {
            this.drawingInProgress = true;
            this.animateDrawing();
        },
        animateDrawing() {
            const interval = setInterval(() => {
                if (this.drawingInProgress) {
                    this.borderWidth += 1;
                    if (this.borderWidth >= 10) {
                        // Điều kiện dừng vẽ (tùy chọn)
                        clearInterval(interval);
                        this.drawingInProgress = false;
                    }
                }
            }, 20); // Điều chỉnh tốc độ vẽ tùy thuộc vào nhu cầu của bạn
        },
        async checkOrder() {
            const params = this.$route.query;
            const dataRes = params.vnp_OrderInfo.split('_')
            if (params.vnp_ResponseCode === '00' && params.vnp_TransactionStatus === '00') {
                // Giao dịch thành công, bạn có thể xử lý dữ liệu khác từ params nếu cần thiết
                await this.completedTransfer(dataRes[0],dataRes[1])
                this.isComplete = true;
                this.expression = 'Đơn hàng của bạn đã hoàn thành!<br>Vào mục vé của bạn để xem thông tin vé đã đặt nhé!'
            } else {
                await this.cancelTransfer(dataRes[0],dataRes[1])
                // Giao dịch thất bại hoặc hủy bỏ
                this.isComplete = false;
                this.expression = 'OOPS!<br>Đơn hàng của bạn không thể thoàn thành vui lòng đặt lại vé!'

            }
            this.isLoading = false;
        },
        letgo() {
            if (!this.isComplete) {
                this.$router.push('/')
            }else{
                this.$router.push('/order')
            }

        },
        async completedTransfer(id,customerId){
            try {
                await axios.put('myticket/api/order/transfer/update-status',
                    {
                        ticketId: id,
                        transferStatus: 5,
                        customerTransferOwnerId: customerId
                    })
            } catch (error) {
                this.$toasted.error('Oops! Đã xảy ra lỗi! Vui lòng thử lại', {
                    position: 'top-right',
                    duration: 3000, // Thời gian hiển thị toast (ms)
                });
            }
        },
        async cancelTransfer(id,customerId){
            try {
                await axios.put('myticket/api/order/transfer/update-status',
                    {
                        ticketId: id,
                        transferStatus: 2,
                        customerTransferOwnerId: customerId
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
    margin: auto;
    color: green;
    border: 1px solid green;
    border-radius: 50%;
    /* Định hình hình tròn */
    transition: border-width 0.5s ease;
    /* Hiệu ứng chuyển động của border */
}

.circle-border-red {
    margin: auto;
    color: red;
    border: 1px solid red;
    border-radius: 50%;
    /* Định hình hình tròn */
    transition: border-width 0.5s ease;
    /* Hiệu ứng chuyển động của border */
}
</style>