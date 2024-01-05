<template>
    <div>
        <div v-if="isLoading">
            <LoadPage />
        </div>
        <div v-if="isSuccess" class="row">
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
                            <select id="gameId" class="form-select" v-model="currentEventselectNow"
                                v-on:change="loadEventData" name="gameId">
                                <option v-for="ticket in currentEvent.eventDetails" :value="ticket.id"
                                    :selected="ticket.id === currentEventselectNow">
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
                <div v-if="currentTicketEvent.eventSeatMapImage !== null" class="row" id="step1">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6 col-xs-6 col-6">
                        <img class="img-seatmap" :src="$fileUrl + currentTicketEvent.eventSeatMapImage" alt="">
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6 col-xs-6 col-6">
                        <form @submit.prevent="submitToCheckout" class="form-ticket-ticket">
                            <input type="hidden" name="_csrf"
                                value="U8hsPodpZMCBzx_wnz8ZE3Bjxw9H1nbsaN6JLp4OqRM9nB9s0lww9NiiV576DUpcElaiYyyFHtsZnfxornjiVw==">
                            <!-- start: Buy Ticket List -->
                            <div style="color: red; float: right;" v-if="!isValueSelected">
                                {{ validate }}
                            </div>
                            <table id="ticketPriceList" class="table table-bordered col-xs-12 mgt-10 text-center">
                                <thead>
                                    <tr>
                                        <th class="col-sm-3">Hạng Vé</th>
                                        <th class="col-sm-3">Giá</th>
                                        <th class="col-sm-3">Số lượng</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for=" (ticketnow, index) in currentTicketEvent.ticketEvents" :key="index"
                                        id="pt-001">
                                        <td class="text-bold ticketpricetext">
                                            {{ ticketnow.name }}</td>
                                        <td>
                                            {{ formatCurrency(ticketnow.price) }}</td>
                                        <td style="display: flex;">
                                            <b-input-group v-if="ticketnow.status === 1">
                                                <b-input-group-prepend>
                                                    <b-button variant="outline-secondary" @click="decrement(ticketnow,index)">
                                                        <b-icon icon="dash-lg"></b-icon>
                                                    </b-button>
                                                </b-input-group-prepend>

                                                <b-form-input type="number"
                                                    class="text-center" style="font-size: 1.2rem;" :min="0"
                                                    :value="order.ticketTypes[index].quantity" disabled
                                                    :max="ticketnow.quantity"></b-form-input>

                                                <b-input-group-append>
                                                    <b-button variant="outline-success" @click="increment(ticketnow,index)">
                                                        <b-icon icon="plus-lg"></b-icon>
                                                    </b-button>
                                                </b-input-group-append>
                                            </b-input-group>
                                            <img style="width: 200px; margin: auto;" v-else-if="ticketnow.status === 3"
                                                :src="$fileUrl + soldOutImg" alt="">
                                                <b-button v-else-if="ticketnow.status === 2"
                                                style="width: 200px; margin: auto;" variant="danger" disabled>Ngừng bán vé</b-button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <div class="row">
                                <div class="col-8" style="max-width: 63.6666667%;">
                                </div>
                                <button type="submit" id="md-viewMap"
                                    class="btn btn-outline-primary col-4 viewmap-btn payment-btn">
                                    Thanh Toán
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
                <div v-else class="row" id="step1">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-xs-12 col-12">
                        <form @submit.prevent="submitToCheckout" class="form-ticket-ticket">
                            <input type="hidden" name="_csrf"
                                value="U8hsPodpZMCBzx_wnz8ZE3Bjxw9H1nbsaN6JLp4OqRM9nB9s0lww9NiiV576DUpcElaiYyyFHtsZnfxornjiVw==">
                            <!-- start: Buy Ticket List -->
                            <div style="color: red; float: right;" v-if="!isValueSelected">
                                {{ validate }}
                            </div>
                            <table id="ticketPriceList" class="table table-bordered col-xs-12 mgt-10 text-center">
                                <thead>
                                    <tr>
                                        <th class="col-sm-3">Hạng Vé</th>
                                        <th class="col-sm-3">Giá</th>
                                        <th class="col-sm-3">Số lượng</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for=" (ticketnow, index) in currentTicketEvent.ticketEvents" :key="index"
                                        id="pt-001">
                                        <td class="text-bold ticketpricetext">
                                            {{ ticketnow.name }}</td>
                                        <td>
                                            {{ formatCurrency(ticketnow.price) }}</td>
                                        <td style="display: flex;">
                                            <b-input-group v-if="ticketnow.status === 1">
                                                <b-input-group-prepend>
                                                    <b-button variant="outline-secondary" @click="decrement(ticketnow,index)">
                                                        <b-icon icon="dash-lg"></b-icon>
                                                    </b-button>
                                                </b-input-group-prepend>

                                                <b-form-input type="number"
                                                    class="text-center" style="font-size: 1.2rem;" :min="0"
                                                    :value="order.ticketTypes[index].quantity" disabled
                                                    :max="ticketnow.quantity"></b-form-input>

                                                <b-input-group-append>
                                                    <b-button variant="outline-success" @click="increment(ticketnow,index)">
                                                        <b-icon icon="plus-lg"></b-icon>
                                                    </b-button>
                                                </b-input-group-append>
                                            </b-input-group>
                                            <img style="width: 200px; margin: auto;" v-else-if="ticketnow.status === 3"
                                                :src="$fileUrl + soldOutImg" alt="">
                                                <b-button v-else-if="ticketnow.status === 2"
                                                style="width: 200px; margin: auto;" variant="danger" disabled>Ngừng bán vé</b-button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <div class="row">
                                <div class="col-8" style="max-width: 65.6666667%;">
                                </div>
                                <button type="submit" id="md-viewMap"
                                    class="btn btn-outline-primary col-4 viewmap-btn payment-btn">
                                    Thanh Toán
                                </button>
                            </div>
                        </form>
                    </div>
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
                                                <option disabled value="">Chọn số lượng
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
                                    Thanh Toán nhé
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
import store from "@/store";
import { MyAsset, TicketStatuses, OrderStatuses } from "@/store/const"
export default {
    props: ['isSelect'],
    components: {
        LoadPage
    },
    data() {
        return {
            quantityTicketType: 0,
            validate: "",
            isValueSelected: true,
            soldOutImg: MyAsset.SOLD_OUT_IMG,
            isLoading: false,
            isSuccess: false,
            isError: false,
            currentEventselected: 0,
            currentEventselectNow: 0,
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
                emittedEvent: false,
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
            order: {
                ticketTypes: [

                ],
                tickets: [

                ]
            }
        }
    },
    mounted() {
        this.fetchDataTicket();
        console.log(this.order);
    },
    watch: {
        'this.$route.params.id': 'fetchDataTicket',
    },
    methods: {
        async loadEventData() {
            this.$router.push({ name: 'buyTicket', params: { type: 'area', id: this.currentEventselectNow } });
            this.fetchDataTicket();
        },
        increment(ticketnow,index) {
            if (this.order.ticketTypes[index].quantity < ticketnow.quantity) {
                this.order.ticketTypes[index].quantity++;
            }
        },
        decrement(ticketnow,index) {
            if (this.order.ticketTypes[index].quantity > 0) {
                this.order.ticketTypes[index].quantity--;
            }
        },
        handleOrder(event, index) {
            if (event.target.value === "0") {
                this.isValueSelected = false;
            } else {
                const orderFind = this.order.ticketTypes
                    .findIndex(item => item.ticketEventId === this.currentTicketEvent.ticketEvents[index].id);
                if (orderFind !== -1) {
                    // Nếu ID đã tồn tại, xóa nó
                    this.order.ticketTypes.splice(orderFind, 1);
                }
                this.order.ticketTypes.push({
                    eventDetailId: this.currentTicketEvent.ticketEvents[index].eventDetailId,
                    ticketEventId: this.currentTicketEvent.ticketEvents[index].id,
                    quantity: event.target.value
                })
                console.log(this.order);
            }
        },
        async submitToCheckout() {
                var countTicket = 0;
                this.order.ticketTypes.forEach(element => {
                    const quantity = parseInt(element.quantity, 10);

                    // Kiểm tra nếu quantity là một số hợp lệ
                    if (!isNaN(quantity)) {
                        countTicket += quantity;
                    }
                });
                if (countTicket > 10) {
                    this.isValueSelected = false;
                    this.validate = "Đơn hàng của bạn chỉ được chọn tối đa 10 vé";
                } else if(countTicket <= 0){
                    this.isValueSelected = false;
                    this.validate = "Chọn ít nhất 1 vé";
                }
                else{
                    await store.commit('setOrderData', this.order);
                    const routeInfo = { name: 'orderTicket', params: { type: 'order' } };
                    this.$router.push({ name: 'orderTicket', params: { type: 'order' }, query: { routeInfo } });
                }
        },
        formatDate(date) {
            // Chuyển đổi ngày thành định dạng dd/mm/yyyy
            return moment(date).format('DD/MM/YYYY');
        },
        formatCurrency(value) {
            return numeral(value).format('0,0') + ' VND';
        },
        async getcurrentTicketEvent(currentDetailId) {
            try {
                const res = await axios.get(
                    "myticket/api/event/detail/find-by-id", {
                    params: {
                        id: currentDetailId
                    },
                    headers: {
                        'Cache-Control': 'no-cache' // Không lưu trữ cache
                    }
                })
                return res.data.data
            } catch (error) {
                console.error('API 1 Error:', error);
                throw error;
            }
        },
        async getEventDetail(currentId) {
            try {
                const res = await axios.get(
                    "/myticket/api/event/find-by-id", {
                    params: {
                        id: currentId
                    }, headers: {
                        'Cache-Control': 'no-cache' // Không lưu trữ cache
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
            if (this.isSelect) {
                try {
                    this.isLoading = true;
                    this.currentTicketEvent = await this.getcurrentTicketEvent(this.$route.params.id);
                    this.currentEventselectNow = this.$route.params.id;
                    this.currentEvent = await this.getEventDetail(this.currentTicketEvent.eventId);
                    if (!this.emittedEvent) {
                        this.$emit('getCurrentEvent', this.currentEvent);
                        this.emittedEvent = true;
                    }
                    this.currentTicketEvent.ticketEvents.forEach(element => {
                        this.order.ticketTypes.push({
                            eventDetailId: this.currentTicketEvent.id,
                            ticketEventId: element.id,
                            quantity: 0
                        })
                    });
                    this.isLoading = false;
                    this.isSuccess = true;
                } catch (error) {
                    this.isLoading = false;
                    this.isError = true;
                    this.$toasted.error(error.response.data.error_description, {
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