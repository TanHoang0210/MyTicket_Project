<template>
 <div style="background-color: #FDF9F7; border-radius: 4px;">
    <section style="display: flex; margin-bottom: 20px;">
    <div class="main__container">
        <div class="main__container--inner">
     <banner></banner>
    <main-event :events="events"></main-event>
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
import MainEvent from "@/components/Home/MainComponent/EventsComponent.vue/MainEvents.vue"
import Banner from "@/components/Home/MainComponent/EventsComponent.vue/Banner.vue"
import Categories from "@/components/Home/MainComponent/Categories.vue"
import Venues from "@/components/Home/MainComponent/Venues.vue"
export default {
    props:['events','categories','venues'],
    components:{MainEvent,Banner,Categories,Venues},
    name:"events"
}
</script>
<style>
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