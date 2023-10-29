<template>
    <div class="venues">
        <h2 class="venue-title">Sân vận động</h2>
        <div class="venue__list row">
            <div class="venue__item--inner col-sm-4 col-xl-2 col-lg-2" v-for="venue in venues">
                <div class="venue__item">
                    <b-card class="mb-0 venue__item--info">
                        <div class="venue__item--img" style="position: relative;">
                            <b-card-img :src="venue.img" alt="Image"></b-card-img>
                            <router-link to="/about">
                                <div class="ticket__button--modal">
                                    <button class="ticket__button">
                                        <h3 class="ticket__button--title">
                                            >
                                        </h3>
                                    </button>
                                </div>
                            </router-link>
                        </div>
                        <b-card-text class="venue__item--type">
                            {{ venue.name }}
                        </b-card-text>
                    </b-card>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
import MainEvent from "@/components/Home/MainComponent/EventsComponent.vue/MainEvents.vue"
import Banner from "@/components/Home/MainComponent/EventsComponent.vue/Banner.vue"
export default {
    props: ['venues'],
    components: { MainEvent, Banner },
    name: "venues"
}
</script>
<style>
.card {
    border: none !important;
}
.venues{
    padding-top: 40px;
}
.main__container--inner {
    margin: auto;
    width: 80%;
}

.venue-title {
    margin-left: 10px;
    font-style: normal;
    font-weight: bold;
    font-size: 1.6rem;
    line-height: 40px;
    align-self: flex-end;
    margin-bottom: 30px;
    color: var(--text-color);

}

.card-body {
    padding: 0 !important;
    border-radius: 5px 5px !important;
}

.venue__list {
    padding: 0;
    margin: 0;
    display: flex;
    justify-content: flex-start;
    flex-wrap: wrap;
}

.venue__item {
    width: 100%;
    height: 100%;
    overflow: hidden;
    display: flex !important;
    margin: auto !important;
}

.venue__item--type {
    text-align: center;
    border-radius: 2px 2px;
    font-weight: 600;
    font-size: 1.2rem;
    bottom: 5px;
    padding: 5px 5px;
}

.venue__item--info {
    height: 100%;
    margin: auto;
    border: none;
    width: 95%;
    position: relative;
}

.venue__item--img {
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
    width: 10%;
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

.venue__item--img:hover .ticket__button--modal {
    display: block;
    cursor: pointer;
}

.ticket__button--modal:hover .ticket__button {
    right: 0;
}

.card-img {
    transition: all ease-out 0.3s !important;
}

.venue__item--img:hover .card-img {
    transform: perspective(800px) translateZ(50px) !important;
}

.ticket__button--modal:hover .ticket__button {
    opacity: 0.8;
    color: var(--primary-color);
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