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
            <div id="promoBox"
                class="alert alert-section text-center col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12 mgb-16">
                <div id="promoDescription" class="inline-block">
                    <div class="mgt-12" id="promo_1321">
                        <div class="text-info promoTitle"> </div>
                        <div class="promoContent"> </div>
                    </div>
                </div>
                <div class="mgt-24 row row-cols-lg-auto g-3 align-items-center align-items-center">
                    <div class="col-lg-12 col-md-12 col-xs-12 col-12">
                        <div class="input-group justify-content-center align-items-center">
                            <div class="form-group promoLabel">Please enter your promo code (if applicable)</div>
                            <div class="form-group promoInput"><input type="text" id="checkCode" class="promoCodeInput"
                                    name="checkCode"
                                    onkeypress="if (event.keyCode == 13) {$(&quot;.check-discount&quot;).trigger(&quot;click&quot;);}">
                                <button type="button" class="btn btn-primary btn-sm check-discount">Submit</button>
                            </div>
                        </div>
                    </div>
                </div>

                <div id="discountCode" class="mgt-16 mgb-16">
                </div>
            </div>
            <div class="panel-heading">
                <div class="step"></div>
                <span>Choose Your Tickets</span>
            </div>
            <div class="row" id="step1">

            </div>
            <div class="row" id="step1">

            </div>
        </div>
    </div>
</template>

<script>
export default {
    props: ['currentEvent', 'listTickets'],
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
    background-image: url(data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 16 16'%3e%3cpath fill='none' stroke='%23343a40' stroke-linecap='round' stroke-linejoin='round' stroke-width='2' d='M2 5l6 6 6-6'/%3e%3c/svg%3e);
    background-repeat: no-repeat;
    background-position: right 0.75rem center;
    background-size: 16px 12px;
    border: 1px solid #ced4da;
    border-radius: 0.25rem;
    transition: border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
    -webkit-appearance: none;
    -moz-appearance: none;
    appearance: none;
}
.panel-heading .step{
    position: absolute;
    margin-top: -16px;
    border-top: 27px inset transparent;
    border-bottom: 27px inset transparent;
    border-left: 15px solid #026CDF;
}
.panel-heading span{
    display: inline-block;
    padding-left: 25px;
    cursor: text;
}
.panel-heading{
    padding: 15px;
    padding-left: 0px;
    text-align: left;
    font-weight: 600;
    border: 1px solid #BFBFBF;
    border-left: 1px solid #BFBFBF;
    border-radius: 4px;
    border-top-left-radius: 0px;
    border-bottom-left-radius: 0px;
}
</style>