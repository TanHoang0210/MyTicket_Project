<template>
  <nav class="home__sidebar">
    <ul class="event__type--list">
      <li class="event__type--item home-action">
        <div class="event__type--itemlink" @click="activeType = !activeType" > 
                 <span title="Home">
                  Thể loại
                </span>
                <div class="show__item--icon">
                  <b-icon-chevron-compact-left class="hidden__item--icon-inner" :class="{ active: activeType}"></b-icon-chevron-compact-left>
                  <b-icon-chevron-compact-down class="show__item--icon-inner" :class="{ active: activeType }"></b-icon-chevron-compact-down>
                </div>

        </div>
      </li>
      <li :class="{ active: activeType }"
      v-for="item in eventtypes" :key="item.id" class="event__type--item">
        <router-link :to="{path:'/event',query:{id: item.id}}" class="event__type--itemlink"> 
                 <span v-bind:title="item.name">{{ item.name }}</span>
        </router-link>
      </li>
    </ul>
    <ul class="event__type--list">
      <li class="event__type--item home-action">
        <div class="event__type--itemlink" @click="activeVenues = !activeVenues" > 
                 <span title="Home">
                  Địa Điểm
                </span>
                <div class="show__item--icon">
                  <b-icon-chevron-compact-left class="hidden__item--icon-inner" :class="{ active: activeVenues }"></b-icon-chevron-compact-left>
                  <b-icon-chevron-compact-down class="show__item--icon-inner" :class="{ active: activeVenues }"></b-icon-chevron-compact-down>
                </div>

        </div>
      </li>
      <li :class="{ active: activeVenues }"
      v-for="item in eventtypes" :key="item.id" class="event__type--item">
        <router-link :to="{path:'/event',query:{id: item.id}}" class="event__type--itemlink"> 
                 <span v-bind:title="item.name">{{ item.name }}</span>
        </router-link>
      </li>
    </ul>
    <div class="mid__border">
    </div>
    <!-- <ul class="user__action--list">
      <li v-for="item in useractions" :key="item.id" class="user__action--item" v-on:click="choiceAction" v-bind:class="{ active: isActive }">
        <router-link :to="{path:'/event',query:{id: item.id}}" class="user__action--itemlink">
            <img class="user__action--img" :src="item.img" v-bind:alt="item.name">  
                 <span v-bind:title="item.name">{{ item.name }}</span>
        </router-link>
      </li>
    </ul> -->
  </nav>
</template>

<script>
export default {
  data() {
    return{
      activeVenues:false,
      activeType:false,
    eventtypes: [{
      id: 1,
      img: 'https://i.ibb.co/nCgKXXQ/musicicon.png',
      name: 'Live Music',
      }, {
      id: 2,
      img: 'https://i.ibb.co/Q623xcY/theatericon.png',
      name: 'Theater - Art Culture',
      }, {
      id: 3,
      img: 'https://i.ibb.co/wC4Zt9N/nightlifeicon.png',
      name: 'Nightlife',
      },{
      id: 4,
      img: 'https://i.ibb.co/hCtf70g/communityicon.png',
      name: 'Community',
      }, {
      id: 5,
      img: 'https://i.ibb.co/qRsznHt/courseicon.png',
      name: 'Course',
      }, {
      id: 6,
      img: 'https://i.ibb.co/xX8XGCY/attractionicon.png',
      name: 'Attractions',
      }, {
      id: 7,
      img: 'https://i.ibb.co/p26K1Zd/sporticon.png',
      name: 'Sport',
      },{
      id: 8,
      img: 'https://i.ibb.co/7KmftBH/Ha-Noiicon.png',
      name: 'Event at Ha Noi',
      },{
      id: 9,
      img: 'https://i.ibb.co/fYRpWKf/HCMicon.png',
      name: 'Event at Ho Chi Minh City',
      }],
    useractions:[
    {
      id: 1,
      img: 'https://i.ibb.co/fX3wp4z/abouticon.png',
      name: 'About Us',
      },
      {
      id: 2,
      img: 'https://i.ibb.co/kB1nVc3/ogannizericon.png',
      name: 'For Organizer',
      },
      {
      id: 3,
      img: 'https://i.ibb.co/Zfdp2KX/FAQicon.png',
      name: 'FAQ',
      },
      {
      id: 4,
      img: 'https://i.ibb.co/QYh5QMR/inforprivacyicon.png',
      name: 'Operational regulations',
      },
      {
      id: 5,
      img: 'https://i.ibb.co/vjRnHdx/inforprivacyicons.png',
      name: 'Information privacy policy',
      },
      {
      id: 6,
      img: 'https://i.ibb.co/GMvqktp/settlementicon.png',
      name: 'Dispute settlement policy',
      },
      {
      id: 7,
      img: 'https://i.ibb.co/zNPxK3y/payprivicyicon.png',
      name: 'Payment privacy policy',
      },
      {
      id: 8,
      img: 'https://i.ibb.co/ySGqPFK/inspectionicon.png',
      name: 'Return and Inspection policy',
      },
      {
      id: 9,
      img: 'https://i.ibb.co/XDQ34gc/deliveryicon.png',
      name: 'Shipping and Delivery conditions',
      },
      {
      id: 10,
      img: 'https://i.ibb.co/KjJjsSC/customertermicon.png',
      name: 'Customer Terms of Use',
      },
      {
      id: 11,
      img: 'https://i.ibb.co/YRRn1mr/organizertermicon.png',
      name: 'Organizer Terms of Use',
      },
      {
      id: 12,
      img: 'https://i.ibb.co/QdN77T4/paymenticon.png',
      name: 'Payment Methods',
      },
    ]
    }
  },
  name:'Navbar'
}
</script>
<style>
    @media only screen and (max-width: 768px) { 
      .home__sidebar{
        display: none;
      }
    }
    .event__type--item.active{
      display: flex !important;
      opacity: 1 ;
    }
    .show__item--icon-inner{
      display: none !important;
    }
    .hidden__item--icon-inner{
      display: block;
    }
    .show__item--icon-inner.active{
      display: block !important;
    }
    .hidden__item--icon-inner.active{
      display: none !important;
    }
  .home__sidebar{
    position: fixed;
    width: 270px;
    top: 92px;
    height: calc(100vh - 76px);
    overflow: hidden;
    background-color: var(--text-color);
    left: 8px;
    border-radius: 4px;
  }
  .home__sidebar:hover{
    overflow: auto;
  }
  /*list event type */
  .event__type--list{
    margin: 0px;
    list-style: none;
    padding: 0 0;
    border-bottom: 1px solid var(--primary-color);
    margin-bottom: 1px;
  }

  .event__type--item{
    display: flex;
    width: 100%;
    font-size: 1em;
    font-style: normal;
    line-height: 40px;
    display: none;
    opacity: 0;
    transition: all 0.1s ease-in;
    text-align: center;
  }
  .event__type--itemlink span{
    margin-left: 16px;
  }
  .event__type--itemlink{
    width: 100%;
    display: flex;
    justify-content: space-between;
    color: var(--primary-color);
    left: 0;
  }
  .event__type--itemlink:hover{
    text-decoration: none;
    color: var(--text-color);
    background-color: var(--primary-color);
  }
  .event__type--item:hover{
    background-color: #fff;
    color: var(--primary-color);
  }
  .show__item--icon{
    align-items: center;
    margin: 0 20px;
    display: flex;
  }
  .home-action{
    font-weight: bold;
    background-color: var(--text-color-color);
    display: block;
    opacity: 1;
  }
  .home-action:hover{
    cursor: pointer;
  }
  .mid__border{
    border-top: 1px solid rgb(230, 235, 245);
    width: calc(254px);
  }
  /* list user action */
  .user__action--list{
    list-style-type: none;
    margin: 0px;
    padding: 0px 32px 16px 0px
  }
  .user__action--img{
    width: 24px;
    height: 24px;
    object-fit: cover;
    margin-right: 8px;
  }
  .user__action--item{
    width: 100%;
    font-size: 14px;
    font-weight: normal;
    margin-left: 16px;
    border-bottom: 1px solid rgb(230, 235, 245);
    line-height: 20px;
    display: flex;
    padding-left: 0px;
  }
  .user__action--itemlink{
    width: 100%;
    height: 100%;
    padding: 4px 8px;
    -webkit-box-align: center;
    align-items: center;
    color: rgb(42, 45, 52);
    display: flex;
  }
  .user__action--itemlink:hover{
    text-decoration: none;
    color: black;
  }
  .user__action--item:hover{
    background-color: #fff;
  }
</style>

<!-- Add "scoped" attribute to limit CSS to this component only -->
