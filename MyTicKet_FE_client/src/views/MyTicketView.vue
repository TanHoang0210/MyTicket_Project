<template>
    <div>
        <div>
            <Header :isLogin="!isLogin"></Header>
        </div>
        <div id="top"></div>
        <main class="mainContent clearfix orderContent OrderList" style="padding-bottom: 30px;">
            <div style="max-width: 1420px;" class="container" id="content">
                <div class="row pt-3">
                    <div class="left-col d-none d-lg-block">
                        <div class="row stickyMenu">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12 ">
                                <ul class="left-sidebar nav nav-navbar flex-column ">
                                    <li class="nav-item">
                                    <li class="nav-item">
                                        <router-link class="nav-link link-nav" to="/user">
                                            Thông tin chung
                                        </router-link>
                                    </li>
                                    <router-link class="nav-link link-nav" to="/order">
                                        Vé của bạn
                                    </router-link>
                                    </li>
                                    <li class="nav-item">
                                        <router-link class="nav-link link-nav" to="/transfer">
                                            Vé chuyển nhượng
                                        </router-link>
                                    </li>
                                    <li class="nav-item">
                                        <router-link class="nav-link link-nav" to="/refund">
                                            Vé trả lại
                                        </router-link>
                                    </li>
                                    <li class="nav-item">
                                        <router-link class="nav-link link-nav" to="#top">
                                            Quay Lại
                                        </router-link>
                                    </li>
                                </ul>
                            </div>
                        </div>

                    </div>
                    <div class="right-col col">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 col-12 mgt-24">
                            <my-ticket v-if="isMyTicket"></my-ticket>
                            <ticket-exchange v-if="isExchangeTicket"></ticket-exchange>
                            <ticket-transfer v-if="isTransferTicket"></ticket-transfer>
                            <my-info-view  v-if="isUserInfo"></my-info-view>
                        </div>
                    </div>
                </div>
            </div>
        </main>
        <div>
        <home-footer :categories="categories"></home-footer>
      </div>
    </div>
</template>

<script>
import MyTicket from "@/components/Home/MyTicketComponent/MyTicket.vue"
import TicketExchange from "@/components/Home/MyTicketComponent/TicketExchange.vue"
import TicketTransfer from "@/components/Home/MyTicketComponent/TicketTransfer.vue"
import MyInfoView from '@/views/MyInfoView.vue'
import Header from "@/components/Header.vue";
import HomeFooter from "@/components/Home/HomeFooter.vue";
export default {
    components: {
        MyTicket, Header, TicketExchange, TicketTransfer,MyInfoView,HomeFooter
    },
    data() {
        return {
            categories:null,
            isMyTicket: false,
            isTransferTicket: false,
            isExchangeTicket: false,
            isUserInfo: false,
        }
    },
    mounted() {
        this.checkRouter();
    },
    watch: {
        '$route.params.type': function (newVal, oldVal) {
            console.log('Route type has changed', newVal);
            this.checkRouter();
        },
    },
    methods: {
        checkRouter() {
            if (this.$route.params.type == 'order') {
                this.isMyTicket = true;
                this.isTransferTicket = false;
                this.isExchangeTicket = false;
                this.isUserInfo = false;

            }
            else if (this.$route.params.type == 'transfer') {
                this.isMyTicket = false;
                this.isTransferTicket = true;
                this.isExchangeTicket = false;
                this.isUserInfo = false;

            }
            else if (this.$route.params.type == 'refund') {
                this.isMyTicket = false;
                this.isTransferTicket = false;
                this.isExchangeTicket = true;
                this.isUserInfo = false;
                
            }
            else if (this.$route.params.type == 'user') {
                this.isMyTicket = false;
                this.isTransferTicket = false;
                this.isExchangeTicket = false;
                this.isUserInfo = true;
            }
        }
    }
}
</script>

<style>
.tb_head {
    background-color: var(--primary-color-bold);
}

.tb_col1 {
    padding: 16px;
}

.stickyMenu {
    position: sticky;
    top: 0px;
}

.nav-link {}

.link-nav {
    color: #262626;
    background: transparent;
    text-align: left;
    border: 0;
    border-bottom: 1px solid #cecece;
    font-weight: 600;
    font-size: 20px;
    width: 100%;
}
</style>