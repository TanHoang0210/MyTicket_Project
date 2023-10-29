<template>
  <div>
    <b-carousel
     ref="myCarousel1"
    id="carousel-1" 
    v-model="slide" 
    indicators 
    background="transparent"
    @sliding-start="onSlideStart" 
    @sliding-end="onSlideEnd">
      <b-carousel-slide v-for="slide in slides">
        <template #img>
          <router-link :to="{ path: '/event/detail', query: { id: slide.id } }" class="slide__link">
            <img style="display: flex; height: 400px; object-fit: fill; margin: auto; border: 1px solid transparent; border-radius: 10px 10px;"
              class="d-block" :src="slide.img" alt="image slot">
          </router-link>
        </template>
      </b-carousel-slide>
    </b-carousel>
    <div style="position: absolute; width: 100%; left: 0; top: 25vh;">
      <a href="#" role="button" @click="prev" aria-controls="carousel-1___BV_inner_" class="carousel-control-prev"><span
          aria-hidden="false" class="carousel-control-prev-icon"></span><span class="sr-only">Previous slide</span></a>
      <a href="#" role="button" @click="next" aria-controls="carousel-1___BV_inner_" class="carousel-control-next"><span
          aria-hidden="true" class="carousel-control-next-icon"></span><span class="sr-only">Next slide</span></a>
    </div>
  </div>
</template>
<script>
export default {
  props: ['slides'],
  data() {
    return {
      slide: this.startSlide(),
      sliding: null,
      imgWidth: '880px'
    }
  }, computed: {
    transformCarousel: function () {
      return this.width;
    }
  },
  methods: {
    onSlideStart(slide) {
      this.sliding = true
    },
    onSlideEnd(slide) {
      this.sliding = false
    },
    prev() {
      this.$refs.myCarousel1.prev()
    },
    next() {
      this.$refs.myCarousel1.next()
    },
    startSlide() {
      return parseInt(this.slides.length / 2);
    },
  }
}
</script>
<style>
#carousel-1 {
  position: relative;
}

.carousel-inner {
  width: 100% !important;
  margin: auto !important;
  display: flex !important;
  overflow: unset !important;
}

.carousel {
  display: flex;
  justify-content: center;
}

.carousel-item {
  display: block !important;
  margin: 0 !important;
  opacity: 0.5 !important;
  transition: all ease-out 0.1s !important;
}

.carousel-item.active {
  opacity: 1 !important;
}

.slide__link {
  margin: auto;
  display: block;
  width: 100% !important;
}

@media only screen and (min-width: 992px) {
  .slide__link img {
    border-radius: 0px !important;
    width: 880px !important;
  }
}

.carousel-control-prev,
.carousel-control-next {
  width: 100px !important;
  height: 100px !important;
  align-items: center;
  margin: auto;
  border-radius: 2px 2px;
  z-index: 0 !important;
}
.carousel-control-prev-icon,
.carousel-control-next-icon{
  width: 40px !important;
  height: 40px !important;
}
.carousel-control-prev {
  left: 5px !important;
  z-index: 0 !important;
  transition: 0.3s !important;
  opacity: 0.8 !important;
}

.carousel-control-next {
  right: 5px !important;
  z-index: 0 !important;
  transition: 0.3s !important;
  opacity: 0.8 !important;
}

.carousel-control-prev:hover,
.carousel-control-next:hover {
  opacity: 1 !important;
}

.carousel-indicators {
  bottom: -60px !important;
}

.carousel-indicators li {
  background-color: var(--primary-color-bold) !important;
  width: 10px !important;
  height: 10px !important;
  margin: 0 5px !important;
  border-radius: 50% !important;
}
.carousel-inner{
transform: translateX(-2304px);
}

</style>