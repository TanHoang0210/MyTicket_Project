<template>
    <div>
        <div v-if="!isList" class="event__list row">
            <div class="event__item--inner col-sm-12 col-xl-4 col-lg-6" v-for="event in events">
                <div class="event__item">
                    <b-card class="mb-3 event__item--info">
                        <div class="event__item--img" style="position: relative;">
                            <b-card-img :src="$fileUrl + event.eventImage" alt="Image"></b-card-img>
                            <router-link :to="{ name: 'eventDetail', query: { id: event.id } }">
                                <div class="ticket__button--modal">

                                </div>
                            </router-link>
                        </div>
                        <b-card-sub-title v-if="event.firstEventDate == event.lastEventDate || event.lastEventDate == null ||event.firstEventDate == null">{{
                            formatDate(event.firstEventDate)
                        }}</b-card-sub-title>
                        <b-card-sub-title v-else-if="event.firstEventDate != event.lastEventDate">{{
                            formatDate(event.firstEventDate) }} - {{formatDate(event.lastEventDate) }}</b-card-sub-title>
                        <b-card-sub-title class="event-name">{{ event.eventName }}</b-card-sub-title>
                    </b-card>
                </div>
            </div>
        </div>
        <div v-else class="event__list row">
            <div class="event__item--inner col-sm-12 col-xl-12 col-lg-12" v-for="event in events">
                <div class="event__item">
                    <b-card class="mb-3 event__item--info list">
                        <div style="display: flex;">
                            <div style="position: relative; width: 250px; margin-right: 10px;">
                                <b-card-img :src="$fileUrl + event.eventImage" alt="Image"></b-card-img>
                            </div>
                            <div>
                            <b-card-sub-title v-if="event.firstEventDate == event.lastEventDate">{{
                                formatDate(event.firstEventDate)
                            }}</b-card-sub-title>
                            <b-card-sub-title v-else-if="event.firstEventDate != event.lastEventDate">{{
                                formatDate(event.firstEventDate) }} - {{
            formatDate(event.lastEventDate) }}</b-card-sub-title>
                            <b-card-sub-title class="event-name">{{ event.eventName }}</b-card-sub-title>
                            <b-card-sub-title v-if="event.firstEventDate == event.lastEventDate">{{
                                event.eventTypeName
                            }}</b-card-sub-title>
                            </div>
                        </div>
                        <div style="display: flex;">
                            <b-button @click="selectTicket(event.id)" class="btn-event" style="margin: auto;" variant="primary">Đặt vé Ngay</b-button>
                        </div>
                    </b-card>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
import moment from 'moment';
export default {
    props: ['events', 'isList'],
    methods: {
        formatDate(date) {
            // Chuyển đổi ngày thành định dạng dd/mm/yyyy
            return moment(date).format('hh:mm  DD/MM/YYYY');
        },
        selectTicket(id){
            this.$router.push({ name: 'eventDetail', query: { id: id } });
        }
    }
}
</script>
<style scoped>
.main__container {
    display: flex;
}
.btn-event{
    background-color: var(--primary-color-bold);
    border: 1px solid var(--primary-color-bold);
}
.btn-event:hover{
    background-color: var(--primary-color-hover-bold);
    border: 1px solid var(--primary-color-hover-bold);
}
.main__container--inner {
    margin: auto;
    width: 80%;
    display: flex;
}

.card-body {
    padding: 0 !important;
    border-radius: 5px 5px !important;
}
.list .card-body{
    display: flex;
    justify-content: space-between;
}
.event-name {
    margin: 5px 5px !important;
    font-weight: 600;
    font-size: 1.2rem;
    color: var(--text-color) !important;
}

.event__item {
    width: 100%;
    height: 100%;
    overflow: hidden;
    display: flex !important;
    margin: auto !important;
}

.event__item--info {
    height: 100%;
    margin: auto;
    border: none;
    width: 100%;
    position: relative;
}

.event__item--img {
    overflow: hidden;
    border-top-left-radius: 5px;
    border-top-right-radius: 5px;
}

.ticket__button--modal {
    top: 0;
    left: -20%;
    position: absolute;
    display: none;
    z-index: 1;
    width: 120%;
    height: 100%;
}

.ticket__button {
    background-color: var(--primary-color-bold);
    color: var(--primary-color);
    position: absolute;
    right: -30px;
    width: 5%;
    height: 100%;
    opacity: 0.1;
    transition: all ease-in 0.3s;
    writing-mode: vertical-rl;
    text-orientation: upright;
    display: flex;
    align-items: center;
    border: none;
    border-radius: none !important;
    opacity: 0.8;
}
.ticket__button:hover {
    opacity: 1;
}

.ticket__button:focus {
    border: none !important;
    background-color: var(--primary-color) !important;
    text-align: center;
}

.ticket__button--title {
    font-size: 1.6em;
}
</style>