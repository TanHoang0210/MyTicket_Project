<template>
    <section id="ticketPrice" class="box-shadow py-4 pb-5 section-content bg-transparent">
            <div class="container">
                <div class="row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12 mgt-24">
                        <h1 class="black-heading">
                            Ticket Pricing
                        </h1>
                        <div  class="prices-content" style="display: flex; flex-direction: row;">
                            <div style="width: 50%; text-align: center;" v-for="(detail,index) in currentEvent.eventDetails">
                                <p>
                                    <strong>
                                        <span style="color: var(--primary-color-hover-bold);">
                                           THÔNG TIN MỞ BÁN VÉ Ngày {{ index + 1 }}
                                        </span>
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
                                        <span  v-for="ticket in detail.ticketEvents">
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
                    </div>
                </div>
            </div>
    </section>
</template>
<script>
import moment from 'moment';
import numeral from 'numeral';
export default {
    props:['currentEvent'],
    methods:{
        formatDate(date) {
      // Chuyển đổi ngày thành định dạng dd/mm/yyyy
      return moment(date).format(' hh:mm DD/MM/YYYY');
    },
    formatCurrency(value) {
            return numeral(value).format('0,0') + ' VND';
        },
    }
}
</script>

<style>
.prices-content{
    font-size: 18px;
    letter-spacing: 1px;
}
</style>