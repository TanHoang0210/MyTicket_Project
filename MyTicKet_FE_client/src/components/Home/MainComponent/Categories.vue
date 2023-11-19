<template>
    <div class="category">
        <h2 class="category-title">Thể Loại</h2>
        <div class="category__list row"  >
            <div class="category__item--inner col-sm-6 col-xl-3 col-lg-3" v-for="category in categories">
            <div class="category__item">
                    <b-card  
              class="mb-0 category__item--info"
              >
              <div class="category__item--img" style="position: relative;">
                  <b-card-img :src="$fileUrl+category.eventTypeImage" alt="Image"></b-card-img>
                  <router-link to="/about">
                <div class="ticket__button--modal">

                </div>
            </router-link>
              </div>
              <b-card-text class="category__item--type">
                {{category.name}}
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
    props:['categories'],
    components:{MainEvent,Banner},
    name:"categories"
}
</script>
<style>
.card{
    border: none !important;
}
.category{
    padding-top: 20px;
}
.main__container--inner{
    margin: auto;
    width: 80%;
}
.category-title{
    margin-left: 10px;
    font-style: normal;
    font-weight: bold;
    font-size: 1.6rem;
    line-height: 40px;
    align-self: flex-end;
    margin-bottom: 30px;
    color: var(--text-color);
}
.card-body{
    padding: 0 !important;
    border-radius: 5px 5px !important;
}
.category__list{
    padding: 0;
    margin: 0;
    display: flex;
    justify-content: flex-start;
    flex-wrap: wrap;
}
.category__item--inner{
    padding-bottom: 10px;
}
.category__item{
    width: 100%;
    height: 100%;
    overflow: hidden;
    display: flex !important;
    margin: auto !important;
    margin-bottom: 10px;
}
.category__item--type{
    position: absolute;
    background-color: rgb(0,0,0,0.7);
    border-radius: 2px 2px;
    color: #fff;
    font-weight: 600;
    font-size: 1.2rem;
    margin: 5px 5px 5px 5px;
    bottom: 5px;
    padding: 5px 5px;
}
.category__item--info{
    height: 100%;
    margin: auto;
    border: none;
    width: 95%;
    position: relative;
}
.category__item--img{
    overflow: hidden;
    border-top-left-radius: 5px;
    border-top-right-radius: 5px;
}
.ticket__button--modal{
    top: 0;
    left: -20%;
    position: absolute;
    display: none;
    z-index: 1;
    width: 120%;
    height: 100%;
}
.ticket__button{
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
.category__item--img:hover .ticket__button--modal{
    display: block;
    cursor: pointer;
}
.ticket__button--modal:hover .ticket__button{
    right: 0;
}
.card-img{
    transition: all ease-out 0.3s !important;
}
.category__item--img:hover .card-img{
    transform: perspective(800px) translateZ(50px) !important;
}
.ticket__button--modal:hover .ticket__button{
    opacity: 0.8;
    color: var(--primary-color);
}
.ticket__button:hover{
    opacity: 1;
}
.ticket__button:focus{
    border: none !important;
    background-color: var(--primary-color) !important;
    text-align: center;
}
.ticket__button--title {
    font-size: 1.6em;
}
</style>