<template>
    <main class="main">
        <div class="main__container">
            <div class="home__container">
                <slide-show :slides="events"></slide-show>
                <events
                 :events="events"
                 :categories="categories"
                 :venues="venues"
                ></events>
            </div>
        </div>
    </main>
</template>
<script>
import Events from "@/components/Home/MainComponent/Events.vue"
import SlideShow from "@/components/Home/MainComponent/SlideShow.vue"
export default{
    props:['categories','events','venues'],
    components:{SlideShow,Events},
    name:'HomeMain'
}
</script>
<style>
.main{
    background-color: var(--primary-color);
    padding-bottom: 24px;
    position: relative;
}
.home__container{
    margin: 0px auto;
    display: flex;
    min-height: 80%;
    height: 80%;
    flex-direction: column;
}
.main__container{
}
@media (max-width: 1200px) { 
    .main__container{
    max-width: 1248px;
    padding-left: 16px;
    padding-right: 16px;
    }

}
@media (max-width: 992px) { 
    .main__container{
    max-width: 960px;
    padding-left: 16px;
    padding-right: 16px;
    }
    .banner__hidden{
        display: none;
     }
}
@media (max-width: 768px) { 
    .main__container{
    max-width: 720px;
    padding-left: 16px;
    padding-right: 16px;
    }
    .main{
            width: 100%;
            margin-left: 0px;
        }
    .carousel-inner{
        width: 80% !important;
     }
     .banner__hidden{
        display: none;
     }
}
@media (max-width: 576px) { 
    .main__container{
    max-width: 540px;
    padding-left: 16px;
    padding-right: 16px;
    }
    .main{
            width: 100%;
            margin-left: 0px;
        }
    .carousel-inner{
        width: 100% !important;
     }
     .banner__hidden{
        display: none;
     }
}
</style>