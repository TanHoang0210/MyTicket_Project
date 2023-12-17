<template>
 <div style="background-color: #FDF9F7; border-radius: 4px;">
    <section style="display: flex; margin-bottom: 20px;">
    <div class="main__container">
        <div class="main__container--inner">
     <banner></banner>
    <main-event :events="events"></main-event>
    <div style="padding-top: 30px;" >
        <h1 class="black-heading">
                  Các sự kiện mới nhất
                </h1>
        <div class="event__list row">
            <div class="event__item--inner col-sm-12 col-xl-4 col-lg-6" v-for="event in eventNews">
                <div class="event__item">
                    <b-card class="mb-3 event__item--info">
                        <div class="event__item--img" style="position: relative;">
                            <b-card-img :src="$fileUrl + event.eventImage" alt="Image"></b-card-img>
                            <router-link :to="{ name: 'eventDetail', query: { id: event.id } }">
                                <div class="ticket__button--modal">

                                </div>
                            </router-link>
                        </div>
                        <b-card-sub-title v-if="event.firstEventDate == event.lastEventDate">{{
                            formatDate(event.firstEventDate)
                        }}</b-card-sub-title>
                        <b-card-sub-title v-else-if="event.firstEventDate != event.lastEventDate">{{
                            formatDate(event.firstEventDate) }} - {{formatDate(event.lastEventDate) }}</b-card-sub-title>
                        <b-card-sub-title class="event-name">{{ event.eventName }}</b-card-sub-title>
                    </b-card>
                </div>
            </div>
        </div>
    </div>
    <div style="padding-top: 30px;">
        <h1 class="black-heading">
                  Các sự kiện bán chạy 
                </h1>
        <div class="event__list row">
            <div class="event__item--inner col-sm-12 col-xl-4 col-lg-6" v-for="event in eventNews">
                <div class="event__item">
                    <b-card class="mb-3 event__item--info">
                        <div class="event__item--img" style="position: relative;">
                            <b-card-img :src="$fileUrl + event.eventImage" alt="Image"></b-card-img>
                            <router-link :to="{ name: 'eventDetail', query: { id: event.id } }">
                                <div class="ticket__button--modal">

                                </div>
                            </router-link>
                        </div>
                        <b-card-sub-title v-if="event.firstEventDate == event.lastEventDate">{{
                            formatDate(event.firstEventDate)
                        }}</b-card-sub-title>
                        <b-card-sub-title v-else-if="event.firstEventDate != event.lastEventDate">{{
                            formatDate(event.firstEventDate) }} - {{formatDate(event.lastEventDate) }}</b-card-sub-title>
                        <b-card-sub-title class="event-name">{{ event.eventName }}</b-card-sub-title>
                    </b-card>
                </div>
            </div>
        </div>
    </div>
    <footer class="main__footer">
        <router-link class="more--btn" to="/event">
            <div class="main__footer--inner">
                <b-button class="main__footer--btn">
                        Xem Thêm
                </b-button>
            </div>
        </router-link>
    </footer>
    <categories :categories="categories"></categories>
    <venues :venues="venues"></venues>
</div>
    </div>
</section>
 </div>
 
</template>
<script>
import moment from "moment"
import MainEvent from "@/components/Home/MainComponent/EventsComponent.vue/MainEvents.vue"
import Banner from "@/components/Home/MainComponent/EventsComponent.vue/Banner.vue"
import Categories from "@/components/Home/MainComponent/Categories.vue"
import Venues from "@/components/Home/MainComponent/Venues.vue"
export default {
    props:['events','categories','venues','eventNews'],
    components:{MainEvent,Banner,Categories,Venues},
    name:"events",
    methods: {
        formatDate(date) {
            // Chuyển đổi ngày thành định dạng dd/mm/yyyy
            return moment(date).format('DD/MM/YYYY');
        },
    },
    mounted(){
        console.log(this.eventNews)
    }
}
</script>
<style>
.card-subtitle {
    margin: 5px 5px !important;
    font-size: 1.2rem !important;
}
.card-img {
    width: 100% !important;
    height: auto !important;
    transition: all ease-out 0.3s !important;
}
.event__item--img:hover .card-img {
    transform: perspective(800px) translateZ(50px) !important;
}
.ticket__button--modal:hover .ticket__button {
    right: 0;
}
.event__list {
    padding: 0;
    margin: 0;
    display: flex;
    justify-content: flex-start;
    flex-wrap: wrap;
}
.event__item--img {
    position: relative;
    width: 100%;
    background-color: inherit;
    background-size: 100%;
    box-shadow: rgba(0, 0, 0, 0.2) 0px 2px 4px;
    border-radius: 2px;
    overflow: hidden;
    background-position: left top;
    text-align: center;
}
.ticket__button--modal:hover .ticket__button {
    right: 0;
}
.event__item--img:hover .ticket__button--modal {
    display: block;
    cursor: pointer;
}

.ticket__button--modal:hover .ticket__button {
    opacity: 0.8;
    color: var(--primary-color);
}
.main__footer{
    margin: auto;
    width: 80%;
    display: flex;
    padding: 30px 0;
}
.main__footer--inner{
    width: 100%;
    display: flex;
    border: 1px var(--primary-color-bold);
}
.more--btn{
    width: 30%;
    display: block;
    margin: auto;
    position: relative;
}
.more--btn:hover{
    text-decoration: none;
}
.main__footer--btn{
    width: 100%;
    background-color: var(--primary-color-bold)!important;
    color: var(--primary-color) !important;
    font-weight: normal !important;
    font-size: 1.5rem !important;
    border-radius: 2px !important;
    border: 1px var(--primary-color-bold);
}
.main__footer--btn:hover{
    background-color: var(--primary-color)!important;
    color: var(--primary-color-bold) !important;
}
.main__footer--btn{
    border: 1px solid var(--primary-color-bold) !important;
}
.more--btn::before{
    content: "";
    width: 100%;
    background-color: var(--primary-color-bold);
    height: 1px;
    position: absolute;
    bottom: 50%;
    left: -110%;
    border-radius: 88px;
}
.more--btn::after{
    content: "";
    width: 100%;
    background-color: var(--primary-color-bold);
    height: 1px;
    position: absolute;
    bottom: 50%;
    right: -110%;
    border-radius: 88px;
}
</style>