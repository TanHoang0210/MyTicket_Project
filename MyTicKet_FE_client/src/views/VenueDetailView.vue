<template>
    <div id="#">
        <div v-if="isLoading">
            <LoadPage />
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
                            <div class="event-banner-img1"
                                v-bind:style="{ backgroundImage: 'linear-gradient(rgba(0, 0, 0, 0.1),rgba(0, 0, 0, 0.9)),url(' + $fileUrl + currentVenue.image + ')' }">
                            </div>
                            <div class="container">
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12">
                                        <div class="masthread-wrap1">
                                            <div class="masthread-banner my-3">

                                            </div>
                                            <h1 class="venue-name">{{ currentVenue.name }}</h1>
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
                                                        <a v-smooth-scroll="{ duration: 1000, offset: -50 }" :id="index"
                                                            :key="item" v-on:click="myChoice(index)"
                                                            v-bind:class="{ activeAction: index == currentAction }"
                                                            class="event-nav-item-link" :href="item.id">
                                                            {{ item.name }}
                                                        </a>
                                                    </li>
                                                </ul>
                                            </div>
                                        </nav>
                                    </div>
                                </div>
                            </div>
                        </section>
                        <about-venue-detail :currentVenue="currentVenue" :isInfo="isInfo"></about-venue-detail>
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
.venue-name{
    color: #fff;
    font-size: 32px;
    margin-bottom: 20px;
    margin-top: 0px;
    text-transform: none;
    line-height: 1.5;
    color: #ffffff;
    font-weight: 600;
    padding: 0px 15px;
    position: absolute;
    bottom: 0;
}
.event-banner-img1 {
    height: 560px;
    position: absolute;
    width: 100%;
    background-size: cover;
    background-position: center;
    background-repeat: no-repeat;
}

.masthread-wrap1 {
    height: 560px;
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

@media only screen and (max-width: 1200px) {
    .middle-nav {
        display: none;
    }
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
import AboutVenueDetail from '@/components/Home/VenueDetailComponent/AboutVenueDetail.vue'
import moment from 'moment';
import LoadPage from "@/views/LoadPage.vue"
import axios from 'axios'
export default {
    name: 'EventDetailView',
    components: {
        Header, HomeFooter, AboutVenueDetail, LoadPage
    },
    data() {
        return {
            isInfo:false,
            isLoading: false,
            isSuccess: false,
            isError: false,
            eventActions: [
                {
                    id: "#eventVenue",
                    name: 'Các sự kiện diễn ra'
                },
                {
                    id: "#venueInfo",
                    name: 'Thông tin chi tiết'
                }
            ],
            currentAction: 0,
            currentVenue: {
                id: 0,
                name: "string",
                address: "string",
                capacity: 0,
                description: "string",
                image: "string",
                eventVenues: [
                    {
                        id: 0,
                        eventName: "string",
                        eventTypeId: 0,
                        eventTypeName: "string",
                        eventDescription: "string",
                        eventImage: "string",
                        admissionPolicy: "string",
                        exchangePolicy: "string",
                        status: 0,
                        firstEventDate: "2023-12-16T11:29:25.725Z",
                        lastEventDate: "2023-12-16T11:29:25.725Z",
                        supplierId: 0,
                        supllier: "string",
                        address: "string",
                        percentSaleTicket: 0
                    }
                ]
            },
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
                    href: '/'
                },
                {
                    text: 'Venue',
                    href: '/venue'
                },
            ]
        }
    },
    mounted() {
        this.fetchData();
    },
    methods: {
        myChoice: function (index) {
            setTimeout(() => {
                this.currentAction = index;
                if(this.currentAction == 0){
                    this.isInfo = false
                }else if(this.currentAction == 1){
                    this.isInfo = true
                }
            }, 500);
        },
        gotoBuy: function () {
            this.currentAction = 4;
            // some code to filter users
        },
        async getVenueDetail() {
            try {
                const res = await axios.get(
                    "myticket/api/venue/find-by-id", {
                    params: {
                        id: this.$route.query.id
                    }
                }
                ).then(res =>
                    this.currentVenue = res.data.data
                )
            } catch (error) {
                console.error('API 1 Error:', error);
                throw error;
            }
        },
        async fetchData() {
            try {
                this.isLoading = true;
                await this.getVenueDetail()
                var currentBread = this.$set(this.currentBread, 'text', this.currentVenue.name);
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
  