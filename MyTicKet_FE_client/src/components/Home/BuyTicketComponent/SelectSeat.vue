<template>
    <div>
        <div v-if="isLoading">
            <LoadPage />
        </div>
    <div v-if="isSuccess"  class="row">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12">
            <div id="pageinfo" class="row py-3">
                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3 col-3 title-img visible-desktop d-none d-sm-block">
                    <img class="img-fluid" :src="$fileUrl + currentEvent.eventImage" alt="">
                </div>
                <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12 col-12">
                    <div class="page-header">
                        <h3 id="seatmapEventName">{{ currentEvent.eventName }}</h3>
                    </div>
                    <div>
                        <select id="gameId" class="form-select" name="gameId">
                            <option v-for="ticket in currentEvent.eventDetails" :value="ticket.id"
                                :selected="ticket.id === currentEventselected">
                                {{ formatDate(ticket.organizationDay) }} -
                                {{ ticket.venueName }} - {{ currentEvent.eventName }}
                            </option>
                        </select>
                    </div>
                </div>
            </div>
            <div class="panel-heading">
                <div class="step"></div>
                <span>Chọn hạng vé của bạn</span>
            </div>
            <div class="row" id="step1">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6 col-xs-6 col-6">
                    <img class="img-seatmap" :src="$fileUrl + currentTicketEvent.eventSeatMapImage" alt="">
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6 col-xs-6 col-6">
                    <form @submit.prevent="false" class="form-ticket-ticket">
                        <input type="hidden" name="_csrf"
                            value="U8hsPodpZMCBzx_wnz8ZE3Bjxw9H1nbsaN6JLp4OqRM9nB9s0lww9NiiV576DUpcElaiYyyFHtsZnfxornjiVw==">
                        <!-- start: Buy Ticket List -->
                        <table id="ticketPriceList" class="table table-bordered col-xs-12 mgt-10 text-center">
                            <thead>
                                <tr>
                                    <th class="col-sm-3">Hạng Vé</th>
                                    <th class="col-sm-3">Giá</th>
                                    <th class="col-sm-3">Số lượng</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="ticketnow in currentTicketEvent.ticketEvents" id="pt-001">
                                    <td class="text-bold ticketpricetext">
                                        {{ ticketnow.name }}</td>
                                    <td>
                                        {{ formatCurrency(ticketnow.price) }}</td>
                                    <td>
                                        <select id="TicketForm_ticketPrice_001" class="w100 form-select"
                                            name="TicketForm[ticketPrice][001]">
                                            <option value="0">Chọn số lượng
                                            </option>
                                            <option v-for="i in ticketnow.quantity" :value="i">{{ i }}
                                            </option>
                                        </select> <input type="hidden" id="TicketForm_priceSize_001"
                                            name="TicketForm[priceSize][001]" value="1">
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <div class="row">
                            <div class="col-8">
                            </div>
                            <button v-on:click="submitToCheckout()" id="md-viewMap"
                                class="btn btn-outline-primary col-4 viewmap-btn payment-btn">
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
    <div v-if="isError" class="row">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12">
            <div id="pageinfo" class="row py-3">
                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3 col-3 title-img visible-desktop d-none d-sm-block">
                    <img class="img-fluid" :src="$fileUrl + currentEvent.eventImage" alt="">
                </div>
                <div class="col-lg-9 col-md-9 col-sm-12 col-xs-12 col-12">
                    <div class="page-header">
                        <h3 id="seatmapEventName">{{ currentEvent.eventName }}</h3>
                    </div>
                    <div>
                        <select id="gameId" class="form-select" name="gameId">
                            <option v-for="ticket in currentEvent.eventDetails" :value="ticket.id"
                                :selected="ticket.id === currentEventselected">
                                {{ formatDate(ticket.organizationDay) }} -
                                {{ ticket.venueName }} - {{ currentEvent.eventName }}
                            </option>
                        </select>
                    </div>
                </div>
            </div>
            <div class="panel-heading">
                <div class="step"></div>
                <span>Chọn hạng vé của bạn</span>
            </div>
            <div class="row" id="step1">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6 col-xs-6 col-6">
                    <img class="img-seatmap" :src="$fileUrl + currentTicketEvent.eventSeatMapImage" alt="">
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6 col-xs-6 col-6">
                    <form @submit.prevent="false" class="form-ticket-ticket">
                        <input type="hidden" name="_csrf"
                            value="U8hsPodpZMCBzx_wnz8ZE3Bjxw9H1nbsaN6JLp4OqRM9nB9s0lww9NiiV576DUpcElaiYyyFHtsZnfxornjiVw==">
                        <!-- start: Buy Ticket List -->
                        <table id="ticketPriceList" class="table table-bordered col-xs-12 mgt-10 text-center">
                            <thead>
                                <tr>
                                    <th class="col-sm-3">Hạng Vé</th>
                                    <th class="col-sm-3">Giá</th>
                                    <th class="col-sm-3">Số lượng</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="ticketnow in currentTicketEvent.ticketEvents" id="pt-001">
                                    <td class="text-bold ticketpricetext">
                                        {{ ticketnow.name }}</td>
                                    <td>
                                        {{ formatCurrency(ticketnow.price) }}</td>
                                    <td>
                                        <select id="TicketForm_ticketPrice_001" class="w100 form-select"
                                            name="TicketForm[ticketPrice][001]">
                                            <option value="0">Chọn số lượng
                                            </option>
                                            <option v-for="i in ticketnow.quantity" :value="i">{{ i }}
                                            </option>
                                        </select> <input type="hidden" id="TicketForm_priceSize_001"
                                            name="TicketForm[priceSize][001]" value="1">
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <div class="row">
                            <div class="col-8">
                            </div>
                            <button v-on:click="submitToCheckout()" id="md-viewMap"
                                class="btn btn-outline-primary col-4 viewmap-btn payment-btn">
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
</div>
</template>

<script>
import LoadPage from "@/views/LoadPage.vue"
import moment from 'moment';
import numeral from 'numeral';
import axios from 'axios';
export default {
    components: {
        LoadPage
    },
    data() {
        return {
            isLoading: false,
            isSuccess: false,
            isError: false,
            currentEventselected: 1,
            selectedEventTicket: null,
            currentTicketEvent: {
                id: 1,
                venueId: 1,
                venueName: "",
                organizationDay: "",
                startSaleTicketDate: "",
                endSaleTicketDate: "",
                eventSeatMapImage: "",
                status: 1,
                ticketEvents: [
                    {
                        id: 1,
                        name: "",
                        price: 1,
                        eventDetailId: 1,
                        status: 1,
                        quantity: 1
                    },
                ]
            },
            currentEvent: {
                id: 8,
                eventName: "",
                eventTypeId: 1,
                eventTypeName: "ConCert",
                eventDescription: "",
                eventImage: "",
                status: 1,
                firstEventDate: "",
                lastEventDate: "",
                eventDetails: [
                    {
                        id: 1,
                        venueId: 1,
                        venueName: null,
                        organizationDay: "",
                        startSaleTicketDate: "",
                        endSaleTicketDate: "",
                        eventSeatMapImage: "",
                        status: 1,
                        ticketEvents: null
                    }
                ]
            },
        }
    },
    mounted() {
        this.fetchDataTicket();
    },
    methods: {
        submitToCheckout: function () {
            this.$router.push({ name: 'user', params: { id: 123 } });
        },
        formatDate(date) {
            // Chuyển đổi ngày thành định dạng dd/mm/yyyy
            return moment(date).format('DD/MM/YYYY');
        },
        formatCurrency(value) {
            return numeral(value).format('0,0') + ' VND';
        },
        async getcurrentTicketEvent() {
            try {
                const res = await axios.get(
                    "myticket/api/event/detail/find-by-id", {
                    params: {
                        id: this.$route.params.id
                    }
                })
                return res.data.data
            } catch (error) {
                console.error('API 1 Error:', error);
                throw error;
            }
        },
        async getEventDetail() {
            try {
                const res = await axios.get(
                    "/myticket/api/event/find-by-id", {
                    params: {
                        id: this.currentTicketEvent.eventId
                    }
                }
                )
                return res.data.data
            } catch (error) {
                console.error('API 1 Error:', error);
                throw error;
            }
        },
        async fetchDataTicket() {
            try {
                this.isLoading = true;
                this.currentTicketEvent = await this.getcurrentTicketEvent();
                this.currentEvent = await this.getEventDetail();
                this.currentEventselected = this.$route.params.id;
                this.$emit('getCurrentEvent', this.currentEvent);
                this.isLoading = false;
                this.isSuccess = true;
            } catch (error) {
                this.isLoading = false;
                this.isError = true;
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
    background-image: url('data:image/svg+xml,%3csvg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 16 16"%3e%3cpath fill="none" stroke="%23343a40" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2 5l6 6 6-6"%3e%3c/path%3e%3c/svg%3e');
    background-position: right 0.75rem center;
    background-size: 16px 12px;
    border: 1px solid #ced4da;
    border-radius: 0.25rem;
    transition: border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
    -webkit-appearance: none;
    -moz-appearance: none;
    appearance: none;
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
}
</style>