<template>
  <div id="#">
    <div v-if="isLoading">
        <LoadPage/>
    </div>
    <div v-if="isSuccess">
    <div>
      <Header :currentUser="currentUser"></Header>
    </div>
    <main class="main" style="padding-bottom: 30px;">
      <div class="main__container">
        <div class="home__container">
          <section class="event-banner adp">
            <b-breadcrumb class="event-breadcrumb" :items="breadItems"></b-breadcrumb>
            <div class="event-banner-img"
              v-bind:style="{ backgroundImage: 'linear-gradient(rgba(0, 0, 0, 0.1),rgba(0, 0, 0, 0.9)),url(' + $fileUrl + currentEvent.eventImage + ')' }">
            </div>
            <div class="container">
              <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12">
                  <div class="masthread-wrap">
                    <div class="masthread-banner my-3">
                      <img class="d-none d-sm-block img-fluid banner__img" :src="$fileUrl + currentEvent.eventImage">
                    </div>
                    <div style="display: flex; justify-content: space-between;" class="masthread-text">
                      <p v-for="detail in currentEvent.eventDetails " class="noPadding">
                        <span class="event-date">
                          {{ formatDate(detail.organizationDay) }}
                        </span>
                        <span> - </span>
                        <span class="event-venue">
                          {{ detail.venueName }}
                        </span>
                      </p>
                    </div>
                    <h1 class="event-name">{{ currentEvent.eventName }}</h1>
                  </div>
                </div>
              </div>
            </div>
          </section>
          <section class="middle-nav">
            <div class="container">
              <div class="row item-center">
                <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12 col-12 pt-3 pt-sm-0">
                  <nav class="navbar-default navbar d-block">
                    <div class="event-navbar-header">
                      <ul class="event-nav-list">
                        <li v-for="(item, index) in eventActions" class="event-nav-item">
                          <a v-smooth-scroll="{ duration: 1000, offset: -50 }" :id="index" :key="item"
                            v-on:click="myChoice(index)" v-bind:class="{ activeAction: index == currentAction }"
                            class="event-nav-item-link" :href="item.id">
                            {{ item.name }}
                          </a>
                        </li>
                      </ul>
                    </div>
                  </nav>
                </div>
                <div style="display: flex;" class="col-lg-3 col-md-3 col-sm-3 col-xs-12 col-12 pt-3 py-sm-2">
                  <a class="btn btn-primary btn-block btn-findTickets nav-ticket"
                    v-smooth-scroll="{ duration: 1000, offset: -60 }" v-on:click="gotoBuy()" href="#ticketList">Chọn Hạng
                    Vé</a>
                </div>
              </div>
            </div>
          </section>
          <about-event-detail :currentEvent="currentEvent" :listTickets="listTickets" :modalShow="modalShow">
          </about-event-detail>
        </div>
      </div>
    </main>
    <div>
      <home-footer :categories="categories"></home-footer>
    </div>
  </div>
  <div v-if="isError">
    <div>
      <Header :currentUser="currentUser"></Header>
    </div>
    <main class="main" style="padding-bottom: 30px;">
      <div class="main__container">
        <div class="home__container">
          <section class="event-banner adp">
            <b-breadcrumb class="event-breadcrumb" :items="breadItems"></b-breadcrumb>
            <div class="event-banner-img"
              v-bind:style="{ backgroundImage: 'linear-gradient(rgba(0, 0, 0, 0.1),rgba(0, 0, 0, 0.9)),url(' + $fileUrl + currentEvent.eventImage + ')' }">
            </div>
            <div class="container">
              <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12">
                  <div class="masthread-wrap">
                    <div class="masthread-banner my-3">
                      <img class="d-none d-sm-block img-fluid banner__img" :src="$fileUrl + currentEvent.eventImage">
                    </div>
                    <div style="display: flex; justify-content: space-between;" class="masthread-text">
                      <p v-for="detail in currentEvent.eventDetails " class="noPadding">
                        <span class="event-date">
                          {{ formatDate(detail.organizationDay) }}
                        </span>
                        <span> - </span>
                        <span class="event-venue">
                          {{ detail.venueName }}
                        </span>
                      </p>
                    </div>
                    <h1 class="event-name">{{ currentEvent.eventName }}</h1>
                  </div>
                </div>
              </div>
            </div>
          </section>
          <section class="middle-nav">
            <div class="container">
              <div class="row item-center">
                <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12 col-12 pt-3 pt-sm-0">
                  <nav class="navbar-default navbar d-block">
                    <div class="event-navbar-header">
                      <ul class="event-nav-list">
                        <li v-for="(item, index) in eventActions" class="event-nav-item">
                          <a v-smooth-scroll="{ duration: 1000, offset: -50 }" :id="index" :key="item"
                            v-on:click="myChoice(index)" v-bind:class="{ activeAction: index == currentAction }"
                            class="event-nav-item-link" :href="item.id">
                            {{ item.name }}
                          </a>
                        </li>
                      </ul>
                    </div>
                  </nav>
                </div>
                <div style="display: flex;" class="col-lg-3 col-md-3 col-sm-3 col-xs-12 col-12 pt-3 py-sm-2">
                  <a class="btn btn-primary btn-block btn-findTickets nav-ticket"
                    v-smooth-scroll="{ duration: 1000, offset: -60 }" v-on:click="gotoBuy()" href="#ticketList">Chọn Hạng
                    Vé</a>
                </div>
              </div>
            </div>
          </section>
          <about-event-detail :currentEvent="currentEvent" :listTickets="listTickets" :modalShow="modalShow">
          </about-event-detail>
        </div>
      </div>
    </main>
    <div>
      <home-footer :categories="categories"></home-footer>
    </div>
  </div>
  </div>
</template>
<style>
.main {
  background-color: var(--primary-color);
  padding-bottom: 0;
  position: relative;
}

.home__container {
  margin: 0px auto;
  display: flex;
  min-height: 100%;
  height: 100%;
  flex-direction: column;
}

.noPadding {
  letter-spacing: 1px;
  color: #fff;
  font-size: 25px;
}

.main__container {}

.event-banner {
  position: relative;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
}

.container {
  width: 150%;
  margin: auto;
}

.item-center {
  align-items: center;
}

.event-name {
  color: #fff;
  font-size: 32px;
  margin-bottom: 20px;
  margin-top: 0px;
  text-transform: none;
  line-height: 1.5;
  color: #ffffff;
  font-weight: 600;
}

.event-banner-img {
  filter: blur(10px);
  position: absolute;
  width: 100%;
  height: 100%;
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat;
}

.masthread-wrap {
  height: auto;
  position: relative;
  padding-top: 30px;
}

.masthread-banner {
  position: relative;
  width: 100%;
  background-color: inherit;
  background-size: 100%;
  box-shadow: rgb(0 0 0 / 20%) 0px 2px 4px;
  border-radius: 2px;
  overflow: hidden;
  background-position: left top;
  text-align: center;
}

.event-breadcrumb {
  position: absolute;
  z-index: 1;
  background-color: transparent !important;
  left: 13%;
}

.breadcrumb-item a {
  color: #fff !important;
}

.breadcrumb-item a:hover {
  color: #fff !important;
}

.breadcrumb-item span {
  color: #fff !important;
}

.banner__img {
  width: 100%;
}

.middle-nav {
  background-color: #fff;
  position: sticky !important;
  top: 0 !important;
  box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15) !important;
  z-index: 1020;
  max-height: 70px;
  background-color: #ffffff !important;
  background: #ffffff;
}

.middle-nav .row::before {
  content: "";
}

.middle-nav .row::after {
  content: "";
}

.navbar-default {
  border: none;
  background-color: transparent !important;
  position: relative;
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  justify-content: space-between;
}

.nav-ticket {
  padding: 10px 0 !important;
  margin: auto;
  background-color: var(--primary-color-bold) !important;
  border: 1px solid var(--primary-color-bold) !important;
}

.nav-ticket:hover {
  background-color: var(--primary-color-hover-bold) !important;
  border: 1px solid var(--primary-color-hover-bold) !important;
}

.py-sm-2 {
  padding-top: 0.5rem !important;
  padding-bottom: 0.5rem !important;
}

.btn-primary {
  font-size: 18px !important;
  line-height: 1.5 !important;
  margin: auto !important;
  font-weight: 600 !important;
}

.event-navbar-header {
  float: none;
  padding: 0;

}

.event-nav-list {
  overflow: hidden;
  padding-left: 0;
  white-space: nowrap;
  width: 100%;
  list-style: none;
  display: block;
  scrollbar-width: none;
  scrollbar-width: none !important;
  margin-bottom: 0;
  position: relative;
}

.event-nav-item {
  display: inline-block;
  margin-right: 10px;
  width: auto;
  text-align: center;
  text-decoration: none;
}

.event-nav-item a {
  font-weight: 600 !important;
  font-size: 18px !important;
  color: var(--text-color) !important;
  margin: 0px !important;
  text-transform: uppercase !important;
  letter-spacing: 2px !important;
  display: block;
  padding: 0.5rem 1rem;
  text-decoration: none;
  transition: color 0.15s ease-in-out, background-color 0.15s ease-in-out, border-color 0.15s ease-in-out;
}

.event-nav-item a:hover {
  text-decoration: none;
  color: #61737C;
}

.activeAction {
  border-bottom: 5px solid var(--primary-color-bold);
}

.bg-grey {
  background-color: #eeeeee;
}
</style>
<script>
import Header from '@/components/Header.vue'
import HomeFooter from '@/components/Home/HomeFooter.vue'
import AboutEventDetail from '../components/Home/EventDetailComponent/AboutEventDetail.vue'
import moment from 'moment';
import LoadPage from "@/views/LoadPage.vue"
import axios from 'axios'
export default {
  name: 'EventDetailView',
  components: {
    Header, HomeFooter, AboutEventDetail,LoadPage
  },
  data() {
    return {
      isLoading: false,
      isSuccess: false,
      isError: false,
      eventActions: [
        {
          id: "#eventDetail",
          name: 'Thông tin chi tiết'
        },
        {
          id: "#ticketPrice",
          name: 'Giá vé'
        },
        {
          id: "#eventRefund",
          name: 'Chính sách đổi trả'
        },
        {
          id: "#eventPolicy",
          name: 'Nội quy sự kiện'
        },
      ],
      currentAction: 0,
      currentEvent: {
        id: 8,
        eventName: "",
        eventTypeId: 1,
        eventTypeName: "ConCert",
        eventDescription: "",
        eventImage: "",
        status: 1,
        firstEventDate: "",
        lastEventDate: "",
        eventDetails: [
          {
            id: 1,
            venueId: 1,
            venueName: null,
            organizationDay: "",
            startSaleTicketDate: "",
            endSaleTicketDate: "",
            eventSeatMapImage: "",
            status: 1,
            ticketEvents: null
          }
        ]
      },
      listTickets: [
        {
          id: 1,
          date: "22/10/2023",
          quantity: 10,
          status: "deactive"
        },
        {
          id: 1,
          date: "23/10/2023",
          quantity: 0,
          status: "active"
        },
        {
          id: 1,
          date: "24/10/2023",
          quantity: 10,
          status: "active"
        }
      ],
      currentBread: {
        text: "",
        active: true
      },
      categories: [
        {
          id: 1,
          img: "https://www.ticketmaster.com/s3images/discovery/Concerts.jpg",
          name: "Buổi hòa nhạc",
        },
        {
          id: 2,
          img: "https://www.ticketmaster.com/s3images/discovery/Sports.jpg",
          name: "Thể thao",
        },
        {
          id: 3,
          img: "https://www.ticketmaster.com/s3images/discovery/Arts.jpg",
          name: "Sân khấu",
        },
        {
          id: 4,
          img: "https://www.ticketmaster.com/s3images/discovery/Family.jpg",
          name: "Giải trí",
        }
      ],
      breadItems: [
        {
          text: 'Home',
          href: '#'
        },
        {
          text: 'Event',
          href: '/event'
        },
      ]
    }
  },
  mounted() {
    this.fetchData();
    console.log(this.currentEvent.eventName);
  },
  methods: {
    myChoice: function (index) {
      setTimeout(() => {
        this.currentAction = index;
      }, 1000);
    },
    gotoBuy: function () {
      this.currentAction = 4;
      // some code to filter users
    },
    async getEventDetail() {
      try {
        const res = await axios.get(
          "/myticket/api/event/find-by-id", {
          params: {
            id: this.$route.query.id
          }
        }
        ).then(res =>
          this.currentEvent = res.data.data
        )
      } catch (error) {
        console.error('API 1 Error:', error);
        throw error;
      }
    },
    async fetchData() {
      try {
        this.isLoading = true;
        await this.getEventDetail()
        var currentBread = this.$set(this.currentBread, 'text', this.currentEvent.eventName);
        this.breadItems.push(currentBread)
        this.isSuccess = true;
        this.isLoading = false;
      } catch (error) {
        this.isLoading = false;
        this.isError = true;
        this.$toasted.error('Oops! Đã xảy ra lỗi! Vui lòng thử lại', {
          position: 'top-right',
          duration: 3000, // Thời gian hiển thị toast (ms)
        });
      }
    },
    formatDate(date) {
      // Chuyển đổi ngày thành định dạng dd/mm/yyyy
      return moment(date).format('DD/MM/YYYY');
    },
  },
  computed: {
    currentUser() {
      return this.$store.getters.currentUser;
    }
  }
}
</script>
