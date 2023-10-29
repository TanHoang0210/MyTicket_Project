<template>
    <div class="main-event">
    <div>
      <Header/>
    </div>
    <div class="about">
      <h1>This is an about page</h1>
    </div>
    <div>
      <home-footer :categories="categories"></home-footer>
    </div>
  </div>
  </template>
  <script>
  import Header from '@/components/Header.vue'
  import HomeFooter from '@/components/Home/HomeFooter.vue'
  export default {
    components: {
      Header, HomeFooter
    }
  }
  </script>
  <style>
  .main-event{
    padding: 0;
    margin: 0;
  }
</style>