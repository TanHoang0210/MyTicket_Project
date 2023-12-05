<template>
  <div style="  border-top: 1px solid var(--primary-color-bold);">
<carousel3d :controls-visible="true" :autoplay="true" :autoplay-timeout="5000">
    <slide v-for="(slide, i) in slides" :index="i">
      <template slot-scope="{ index, isCurrent, leftIndex, rightIndex }">
          <router-link :to="{ name: 'eventDetail', query: { id: slide.id }} ">
            <img :data-index="index" :class="{ current: isCurrent, onLeft: (leftIndex >= 0), onRight: (rightIndex >= 0) }" :src="$fileUrl+slide.eventImage">
          </router-link>
        </template>
    </slide>
</carousel3d>
  </div>
</template>
<script>
import { Carousel3d, Slide } from 'vue-carousel-3d';
export default {
  components:{
    Carousel3d,
    Slide
  },
  props: ['slides'],
  mounted() {
    // console.log(slides)
    console.log('hahaha')
  },
}
</script>
<style>
.carousel-3d-container{
  height: 410px !important;
  margin: 10px auto !important;
  padding: 10px;
}
.carousel-3d-slide{
  width: 980px !important;
  height: 410px !important;
  background-color: transparent !important;
  border: none !important;
}
.carousel-3d-slider{
  width: 65% !important;

}
</style>