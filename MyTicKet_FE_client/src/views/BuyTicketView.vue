<template>
    <div>
        <div>
            <Header />
        </div>
        <div style="display: block; height: 50px; background-color: var(--primary-color-bold); margin-bottom:20px ;">
            <b-breadcrumb class="event-breadcrumb black-breadcrumb" :items="breadItems"></b-breadcrumb>
        </div>
        <main class="main" style="margin-bottom: 30px;">
            <div class="main__container">
                <div class="home__container">
                    <div class="container">
                        <div class="row" id="mobileWizard">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12">
                                <div class="purchaseProgress">
                                    <div class="progress-track"></div>
                                    <div
                                     :class="{
                                        'active': isSelect,
                                        '': !isSelect,
                                        'done':isComfirm || isComplete || isPaying
                                        }"
                                     class="progress-step">
                                        Chọn chỗ
                                    </div>
                                    <div 
                                    :class="{
                                        'active': isComfirm,
                                        '': !isComfirm,
                                        'done':isPaying || isComplete
                                        }"
                                    class="progress-step">
                                        Xác minh
                                    </div>
                                    <div 
                                    :class="{
                                        'active': isPaying,
                                        '': !isPaying,
                                        'done':isComplete
                                        }"
                                    class="progress-step">
                                        Thanh toán
                                    </div>
                                    <div
                                    :class="{
                                        'active': isComplete,
                                        '': !isComplete,
                                        }"
                                     class="progress-step">
                                        Nhận vé
                                    </div>
                                </div>
                            </div>
                        </div>
                        <select-seat v-if="isSelect" :currentEvent="currentEvent" :listTickets="listTickets"></select-seat>
                    </div>
                </div>
            </div>
        </main>
        {{ urlParams }}
        <div>
            <home-footer :categories="categories"></home-footer>
        </div>
    </div>
</template>

<script>
import Header from '@/components/Header.vue'
import HomeFooter from '@/components/Home/HomeFooter.vue'
import SelectSeat from '@/components/Home/BuyTicketComponent/SelectSeat.vue'
export default {
    name: 'BuyTicketView',
    components: {
        Header, HomeFooter, SelectSeat
    },
    data() {
        return {
            isSelect: false,
            isComfirm: false,
            isPaying: false,
            isComplete: false,
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
                }
            ],
            preBread: {
                text: "",
                href: '/event/detail/id=5'
            },
            currentBread: {
                text: "",
                active: true
            },
            currentEvent: {
                id: 6,
                img: "https://static.ticketmaster.sg/images/activity/23_euphony2023_28b732c998089649a3ae8202af802888.jpg",
                name: "Kịch IDECAF: Sắc Màu",
                date: '22/10/2023',
                type: 'Theater',
                venue: 'Sân vận động QK7',
            },
            listTickets: [
                {
                    id: 1,
                    date: "22/10/2023",
                    quantity: 10,
                    status: "deactive"
                },
                {
                    id: 2,
                    date: "23/10/2023",
                    quantity: 0,
                    status: "active"
                },
                {
                    id: 3,
                    date: "24/10/2023",
                    quantity: 10,
                    status: "active"
                }
            ],
        }
    },
    mounted() {
        var breadPre = this.$set(this.preBread, 'text', this.currentEvent.name);
        this.breadItems.push(breadPre)
        var breadActive = this.$set(this.currentBread, 'text', 'Chọn chỗ');
        this.breadItems.push(breadActive)
        if(this.$route.params.section == 'area'){
            this.isSelect = true;
        }
    }
}
</script>

<style>
.main {
    background-color: #fff;
}

.black-breadcrumb a {
    color: var(--text-color) !important;
}

.black-breadcrumb span {
    color: var(--text-color) !important;
}

.mobileWizard {
    margin-top: 24px;
}

.purchaseProgress {
    position: relative;
    display: flex;
    color: #000000;
    height: auto;
    background: transparent;
    border: none;
    box-shadow: none;
    margin-bottom: 20px;
}

.progress-track {
    position: absolute;
    top: 9px;
    width: 100%;
    height: 8px;
    z-index: 0;
}

.progress-step {
    font-size: 16px;
    position: relative;
    width: 100%;
    text-align: center;
    z-index: 1;
}

.progress-step.active::before {
    font-family: "FontAwesome";
    content: "\f00c";
    display: flex;
    margin: 0 auto;
    margin-bottom: 10px;
    width: 30px;
    height: 30px;
    background-image: linear-gradient(90deg, var(--primary-color-hover-bold), var(--primary-color-bold));
    border: none;
    border-radius: 100%;
    color: #fff;
    align-items: center;
    text-align: center;
    justify-content: center;
}

.progress-step.done::before {
    font-family: "FontAwesome";
    content: "\f00c";
    display: flex;
    margin: 0 auto;
    margin-bottom: 10px;
    width: 30px;
    height: 30px;
    background: var(--primary-color-hover-bold);
    border: none;
    border-radius: 100%;
    color: #fff;
    align-items: center;
    text-align: center;
    justify-content: center;
    transition: all 1s ease-in-out;
}
.progress-step.disable::before {
    font-family: "FontAwesome";
    content: "\f00c";
    display: flex;
    margin: 0 auto;
    margin-bottom: 10px;
    width: 30px;
    height: 30px;
    background-image: #c8c8c8;
    border: none;
    border-radius: 100%;
    color: #fff;
    align-items: center;
    text-align: center;
    justify-content: center;
}
.progress-step::before {
    font-family: "FontAwesome";
    content: " ";
    display: flex;
    margin: 0 auto;
    margin-bottom: 10px;
    width: 30px;
    height: 30px;
    background: #c8c8c8;
    border: 2px solid #c8c8c8;
    border-radius: 100%;
    color: #fff;
    align-items: center;
    text-align: center;
    justify-content: center;
}

.progress-step:not(:last-of-type)::after {
    content: "";
    position: absolute;
    top: 12px;
    left: 50%;
    width: 0%;
    transition: width 1s ease-in;
    height: 5px;
    background: #c8c8c8;
    z-index: -1;
    width: 100%;
}

.progress-step:not(:last-of-type).done::after {
    content: "";
    position: absolute;
    top: 12px;
    left: 50%;
    width: 0%;
    transition: width 1s ease-in;
    height: 5px;
    background: var(--primary-color-hover-bold);
    z-index: -1;
    width: 100%;
}
</style>