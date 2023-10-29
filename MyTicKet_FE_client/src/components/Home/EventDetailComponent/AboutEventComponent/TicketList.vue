<template>
    <section id="ticketList" class="bg-grey">
        <div class="container">
            <div id="eventListing" class="row pb-3">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12 mgt-16 pt-3 pb-3 pb-sm-5">
                    <h1 class="black-heading">EVENTS</h1>
                    <div id="step-container" class="panel-group">
                        <div id="single-event">
                            <div id="step-2a" class="panel panel-default table-view">
                                <div class="panel-heading">
                                    <div>
                                        Chọn sự kiện bạn muốn mua theo danh sách sau nhé!.
                                    </div>
                                </div>
                                <div class="panel-body">
                                    <div id="singleEvent" class="gameList">
                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12">
                                                <div id="gameList_general" class="grid-view">
                                                    <div id="gameList" class="grid-view">
                                                        <table class="table table-bordered col-xs-12 col-12 mgb-0">
                                                            <thead>
                                                                <tr>
                                                                    <th>Thời Gian</th>
                                                                    <th>Tên sự kiện</th>
                                                                    <th>Địa điểm</th>
                                                                    <th>Trạng thái</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <tr v-for="ticket in listTickets" class="text-center">
                                                                    <td>{{ ticket.date }}</td>
                                                                    <td> {{ currentEvent.name }}</td>
                                                                    <td> {{ currentEvent.venue }}</td>
                                                                    <td>
                                                                        <b-button
                                                                            v-if="ticket.status == 'active' && ticket.quantity > 0"
                                                                            variant="primary" class="button-event ">
                                                                            <router-link class="buy-btn" to="/ticket/area" style="color: #fff; !important">
                                                                                Mua
                                                                                vé
                                                                            </router-link>
                                                                        </b-button>

                                                                        <b-button
                                                                            v-else-if="ticket.status == 'active' && ticket.quantity <= 0"
                                                                            variant="secondary"
                                                                            class="button-event btn-primary" disabled>Đã bán
                                                                            hết</b-button>


                                                                        <img v-else-if="ticket.status == 'deactive'"
                                                                            class="sold-out"
                                                                            src="https://i.ibb.co/LrT0jLQ/soldout.png"
                                                                            alt="">
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</template>
<script>
export default {
    name: "TicketList",
    props: ['currentEvent', 'listTickets'],
}
</script>
<style>
.buy-btn:hover{
    text-decoration: none;
}
.black-heading {
    line-height: 1.5;
    position: relative;
    font-size: 22px;
    margin-bottom: 24px;
    letter-spacing: 2px;
    font-weight: 700;
    text-transform: uppercase;
    padding-bottom: 15px;
    color: var(--text-color);
}

.black-heading::before {
    position: absolute;
    bottom: 0px;
    display: block;
    width: 40px;
    content: "";
    border-bottom: 4px solid var(--text-color);
}

.panel-group {
    border-radius: 4px;
    background-color: rgb(255, 255, 255);
    box-shadow: rgb(0 0 0 / 15%) 0px 0px 10px 3px;
}

.panel-heading {
    font-weight: 600;
    font-size: 18px;
    border: 0;
    border-radius: 0;
    display: flex;
    justify-content: space-between;
    padding: 10px 15px;
    background-color: #EBEBEB;
}

.panel-body {
    padding: 15px;
}

.game-list {
    padding-left: 15px;
    padding-right: 15px;
    margin-bottom: 8px;
}

.table-bordered {
    width: 100%;
    border: 1px solid #BFBFBF;
}

.table-bordered>thead {
    vertical-align: bottom;
    border-color: inherit;
    border-style: solid;
    border-width: 0;
}

.table-bordered>thead>tr>th {
    padding: 16px;
    border-bottom: none;
    font-size: 18px;
    vertical-align: middle;
    border: 1px solid #BFBFBF;
    text-align: center;
    background: #EBEBEB;
    font-weight: bold !important;
}

.text-center {
    text-align: center !important;
}

.table-bordered>tbody>tr>td {
    padding: 8px;
    font-size: 18px;
    vertical-align: middle;
}

.button-event {
    padding: 10px 20px;
    font-size: 20px;
}

.sold-out {
    width: 120px;
}

.button-event {
    width: 120px;
    height: 53px;
}</style>