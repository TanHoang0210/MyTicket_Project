<template>
    <div>
        <main class="main" style="padding-bottom: 30px;">
            <div class="main__container">
                <div class="home__container">
                    vé trả lại
                </div>
            </div>
        </main>
    </div>
</template>
<script>
import axios from 'axios'
import moment from 'moment'
import numeral from 'numeral'
export default {
    data() {
        return {
            tickets: [
                {
                    id: 11,
                    orderId: 2,
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
        async getOrderInfo() {

        },
        async fetchData() {

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
        
    },
}
</script>
<style></style>