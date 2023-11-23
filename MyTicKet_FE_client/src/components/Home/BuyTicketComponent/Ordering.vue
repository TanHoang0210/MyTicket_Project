<template>
    <div>
        <div>
            <Header :isLogin="!isLogin"></Header>
        </div>
        <LoadPage />
    </div>
</template>
<script>
import LoadPage from "@/views/LoadPage.vue"
import Header from "@/components/Header.vue";
import axios from "axios";
export default {
    name: 'BuyTicketView',
    components: {
        LoadPage, Header
    },
    data() {
        return {
            orderInfo: {
                id: 0,
                orderCode: "string",
                customerId: 0,
                status: 0,
                orderDate: "2023-11-22T14:52:42.877Z",
                qrCode: "string",
                total: 0,
                orderDetails: [
                    {
                        id: 0,
                        orderId: 0,
                        eventDetailId: 0,
                        eventName: "string",
                        organizationDay: "2023-11-22T14:52:42.877Z",
                        venueName: "string",
                        ticketId: 0,
                        ticketEventName: "string",
                        ticketCode: "string",
                        seatCode: "string",
                        price: 0
                    }
                ]
            }
        }
    },
    mounted() {
        this.postOrder()
    },
    methods: {
        async orderTicket() {
            try {
                const res = await axios.post('myticket/api/order/create', this.$store.state.orderFormData)
                return res.data.data;
            } catch (error) {
                this.$toasted.error('Oops! Đã xảy ra lỗi! Vui lòng thử lại', {
                    position: 'top-right',
                    duration: 3000, // Thời gian hiển thị toast (ms)
                });
            }
        },
        async postOrder() {
            try {
                this.orderInfo = await this.orderTicket();
                const routeInfo = { name: 'checkout', params: { type: 'checkout', id: this.orderInfo.id }};
                this.$router.push({ name: 'checkout' , params: { type: 'checkout', id: this.orderInfo.id }, query: { routeInfo } });
            } catch (error) {
                this.$router.push({ name: 'buyTicket', params: { type: 'area', id: this.orderInfo.orderDetails[0].eventDetailId } });
            }
        }
    }
}
</script>