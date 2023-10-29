<template>
        <div class="event__list row"  >
            <div class="event__item--inner col-sm-12 col-xl-4 col-lg-6" v-for="event in events">
            <div class="event__item">
                    <b-card  
              class="mb-3 event__item--info"
              >
              <div class="event__item--img" style="position: relative;">
                  <b-card-img :src="event.img" alt="Image"></b-card-img>
                  <router-link :to="{ path: '/event/detail', query: { id: event.id } }">
                <div class="ticket__button--modal">
                    <button class="ticket__button">
                        <h3 class="ticket__button--title">
                            >
                        </h3>
                    </button>
                </div>
            </router-link>
              </div>
              <b-card-sub-title>{{ event.date }}</b-card-sub-title>
              <b-card-sub-title class="event-name">{{event.name}}</b-card-sub-title>
      </b-card>
            </div>
            </div>
        </div>
</template>
<script>
export default{
    props:['events']
}
</script>
<style scoped>
.main__container{
    display: flex;
}
.main__container--inner{
    margin: auto;
    width: 80%;
    display: flex;
}
.card-body{
    padding: 0 !important;
    border-radius: 5px 5px !important;
}
.card-subtitle{
    margin: 5px 5px !important;
    font-size: 1.2rem !important;
}
.event__list{
    padding: 0;
    margin: 0;
    display: flex;
    justify-content: flex-start;
    flex-wrap: wrap;
}
.event-name{
    margin: 5px 5px !important;
    font-weight: 600;
    font-size: 1.2rem;
    color: var(--text-color) !important;
}
.event__item{
    width: 100%;
    height: 100%;
    overflow: hidden;
    display: flex !important;
    margin: auto !important;
}
.event__item--info{
    height: 100%;
    margin: auto;
    border: none;
    width: 100%;
    position: relative;
    margin-bottom: 5px;
}
.event__item--img{
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
.event__item--img:hover .ticket__button--modal{
    display: block;
    cursor: pointer;
}
.ticket__button--modal:hover .ticket__button{
    right: 0;
}
.card-img{
    transition: all ease-out 0.3s !important;
}
.event__item--img:hover .card-img{
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