<template>
 <div class="home__banner">
    <div class="home__banner--inner">
        <img
            class="banner__img banner__hidden"
            :src="banner2"
            alt="banner img"
            >
        <h2 class="slogan__banner">
            Sự kiện nổi bật
        </h2>
        <div class="banner__bottom--border">
        </div>
        <img
            class="banner__img"
            :src="banner1"
            alt="banner img"
            >
    </div>
</div>
</template>
<script>
export default {
    data() {
      return{ 
        banner1:"https://i.ibb.co/tzGmvst/event.png",
        banner2:"https://i.ibb.co/4gCRBXy/event2.png"
      }
    }
    }
</script>
<style scoped>
.home__banner{
    text-align: center;
    display: block;
    margin: 24px 0;
    color: var(--text-color);

}
.home__banner--inner{
    display: inline-flex;
    align-items: center;
    -webkit-box-align: center;
    flex-wrap: wrap;
    justify-content: space-around;
    position: relative;
    -webkit-box-pack: justify;
}
.slogan__banner{
    font-style: normal;
    font-weight: bold;
    font-size: 32px;
    line-height: 40px;
    align-self: flex-end;
    margin-bottom: 30px;
}
.banner__img{
    width: 150px;
}
.banner__hidden{
    margin-bottom: -20px;
}
.banner__bottom--border{
    width: 100%;
    background-color: var(--primary-color-bold);
    position: absolute;
    height: 1px;
    bottom: 15px;
    border-radius: 88px;
}
.home__banner--inner::before{
    content: "";
    width: 40px;
    background-color: var(--primary-color-bold);
    height: 1px;
    position: absolute;
    left: -48px;
    bottom: 15px;
    border-radius: 88px;
}
.home__banner--inner::after{
    content: "";
    width: 40px;
    background-color: var(--primary-color-bold);
    height: 1px;
    position: absolute;
    right: -48px;
    bottom: 15px;
    border-radius: 88px;
}
</style>