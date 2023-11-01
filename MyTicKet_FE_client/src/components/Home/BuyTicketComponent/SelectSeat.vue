<template>
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12">
            <div id="pageinfo" class="row py-3">
                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3 col-3 title-img visible-desktop d-none d-sm-block">
                    <img class="img-fluid" :src="currentEvent.img" alt="">
                </div>
                <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12 col-12">
                    <div class="page-header">
                        <h3 id="seatmapEventName">{{ currentEvent.name }}</h3>
                    </div>
                    <div>
                        <select id="gameId" class="form-select" name="gameId">
                            <option v-for="ticket in listTickets" :value="ticket.id">{{ ticket.date }} -
                                {{ currentEvent.venue }} - {{ currentEvent.name }}</option>
                        </select>
                    </div>
                </div>
            </div>
            {{ time }}
            <div class="panel-heading">
                <div class="step"></div>
                <span>Chọn hạng vé của bạn</span>
            </div>
            <div class="row" id="step1">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6 col-xs-6 col-6">
                    <img class="img-seatmap"
                        src="https://static.ticketmaster.sg/images/activity/field/23_euphony2023_dd028f21b48dced710f997c10541d4c0.jpg"
                        alt="">
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6 col-xs-6 col-6">
                    <form action="/ticket/order" method="post" class="form-ticket-ticket">
                        <input type="hidden" name="_csrf"
                            value="U8hsPodpZMCBzx_wnz8ZE3Bjxw9H1nbsaN6JLp4OqRM9nB9s0lww9NiiV576DUpcElaiYyyFHtsZnfxornjiVw==">
                        <!-- start: Buy Ticket List -->
                        <table id="ticketPriceList" class="table table-bordered col-xs-12 mgt-10 text-center">
                            <thead>
                                <tr>
                                    <th class="col-sm-3">Hạng Vé</th>
                                    <th class="col-sm-2">Giá</th>
                                    <th class="col-sm-2">Số lượng</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr id="pt-001">
                                    <td class="text-bold ticketpricetext">
                                        CAT 1 - SITTING</td>
                                    <td>
                                        1.000.000 vnđ </td>
                                    <td>
                                        <select id="TicketForm_ticketPrice_001" class="w100 form-select"
                                            name="TicketForm[ticketPrice][001]">
                                            <option value="0">Chọn số lượng</option>
                                            <option value="1">1</option>
                                            <option value="2">2</option>
                                            <option value="3">3</option>
                                            <option value="4">4</option>
                                            <option value="5">5</option>
                                            <option value="6">6</option>
                                            <option value="7">7</option>
                                            <option value="8">8</option>
                                            <option value="9">9</option>
                                            <option value="10">10</option>
                                        </select> <input type="hidden" id="TicketForm_priceSize_001"
                                            name="TicketForm[priceSize][001]" value="1">
                                    </td>
                                </tr>
                                <tr id="pt-003">
                                    <td class="text-bold ticketpricetext">
                                        CAT 2 - STANDING</td>
                                    <td>
                                        800.000 vnđ</td>
                                    <td>
                                        <select id="TicketForm_ticketPrice_003" class="w100 form-select"
                                            name="TicketForm[ticketPrice][003]">
                                            <option value="0">Chọn số lượng</option>
                                            <option value="1">1</option>
                                            <option value="2">2</option>
                                            <option value="3">3</option>
                                            <option value="4">4</option>
                                            <option value="5">5</option>
                                            <option value="6">6</option>
                                            <option value="7">7</option>
                                            <option value="8">8</option>
                                            <option value="9">9</option>
                                            <option value="10">10</option>
                                        </select> <input type="hidden" id="TicketForm_priceSize_003"
                                            name="TicketForm[priceSize][003]" value="1">
                                    </td>
                                </tr>
                                <tr id="pt-004">
                                    <td class="text-bold ticketpricetext">
                                        CAT 3 - SITTING </td>
                                    <td>
                                        600.000 </td>
                                    <td>
                                        <select id="TicketForm_ticketPrice_004" class="w100 form-select"
                                            name="TicketForm[ticketPrice][004]">
                                            <option value="0">Chọn số lượng</option>
                                            <option value="1">1</option>
                                            <option value="2">2</option>
                                            <option value="3">3</option>
                                            <option value="4">4</option>
                                            <option value="5">5</option>
                                            <option value="6">6</option>
                                            <option value="7">7</option>
                                            <option value="8">8</option>
                                            <option value="9">9</option>
                                            <option value="10">10</option>
                                        </select> <input type="hidden" id="TicketForm_priceSize_004"
                                            name="TicketForm[priceSize][004]" value="1">
                                    </td>
                                </tr>
                                <tr id="pt-005">
                                    <td class="text-bold ticketpricetext">
                                        CAT 4 - STANDING</td>
                                    <td>
                                        500.000 vnđ</td>
                                    <td>
                                        <select id="TicketForm_ticketPrice_005" class="w100 form-select"
                                            name="TicketForm[ticketPrice][005]">
                                            <option value="0">Chọn số lượng</option>
                                            <option value="1">1</option>
                                            <option value="2">2</option>
                                            <option value="3">3</option>
                                            <option value="4">4</option>
                                            <option value="5">5</option>
                                            <option value="6">6</option>
                                            <option value="7">7</option>
                                            <option value="8">8</option>
                                            <option value="9">9</option>
                                            <option value="10">10</option>
                                        </select> <input type="hidden" id="TicketForm_priceSize_005"
                                            name="TicketForm[priceSize][005]" value="1">
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <div class="row">
                            <div class="col-8">
                            </div>
                            <button id="md-viewMap" class="btn btn-outline-primary col-4 viewmap-btn payment-btn">
                                Thanh Toán
                            </button>
                        </div>
                    </form>
                </div>
            </div>
            <div class="row" id="step1">

            </div>
        </div>
    </div>
</template>

<script>
export default {
    props: ['currentEvent', 'listTickets'],
    data() {
        return {
            
        }
    }
  }
</script>

<style>
.page-header h3 {
    font-weight: 600;
    font-size: 24px;
    display: inline-block;
    float: none;
    margin-bottom: 5px;
}

.page-header {
    border-bottom: 0;
    margin-bottom: 16px;
    padding: 0;
}

.form-select {
    display: block;
    width: 100%;
    padding: 0.375rem 2.25rem 0.375rem 0.75rem;
    -moz-padding-start: calc(0.75rem - 3px);
    font-size: 1rem;
    font-weight: 400;
    line-height: 1.5;
    color: #212529;
    background-color: #fff;
    background-repeat: no-repeat;
    background-position: right 0.75rem center;
    background-size: 16px 12px;
    border: 1px solid #ced4da;
    border-radius: 0.25rem;
    transition: border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
    -webkit-appearance: none;
    -moz-appearance: none;
    appearance: none;
    background-image: url();
}

.panel-heading .step {
    position: absolute;
    margin-top: -10px;
    border-top: 23px inset transparent;
    border-bottom: 25px inset transparent;
    border-left: 15px solid var(--primary-color-bold);
}

.panel-heading span {
    display: inline-block;
    padding-left: 25px;
    cursor: text;
}

.panel-heading {
    padding: 10px;
    padding-left: 0px;
    text-align: left;
    font-weight: 600;
    border: 1px solid #BFBFBF;
    border-left: 1px solid #BFBFBF;
    border-radius: 4px;
    border-top-left-radius: 0px;
    border-bottom-left-radius: 0px;
}

#step1 {
    margin-top: 20px;

}

.img-seatmap {
    width: 100%;
}

.payment-btn {
    right: 0;
}</style>