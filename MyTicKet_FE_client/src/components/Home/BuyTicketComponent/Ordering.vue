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
import store from "@/store";
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
                console.log(store.getters.orderFormData)
                const res = await axios.post('myticket/api/order/create', store.getters.orderFormData, {
                    headers: {
                        'Content-Type': 'application/json',
                    },
                });
                if (res.data.code === 200) {
                    this.$router.push({ name: 'checkout', params: { type: 'checkout', id: res.data.data.id } });
                } else {
                    window.history.back()
                    this.$toasted.error(res.data.message, {
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
        async postOrder() {
            this.orderInfo = await this.orderTicket();
        }
    }
}
</script>