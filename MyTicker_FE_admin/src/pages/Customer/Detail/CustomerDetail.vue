<template>
    <div class="content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-12">
                    <card class="strpied-tabled-with-hover" body-classes="table-full-width table-responsive">
                        <template slot="header">
                            <div class="row" style="display: flex; justify-content: space-between;">
                                <div class="col-md-12">
                                    <h4 class="card-title">Thông tin chi tiết khách hàng</h4>
                                    <ul style="display: flex;  list-style: none; margin-left: -40px;">
                                        <li class="tab-item">
                                            <router-link class="tab-info" id="info" :class="{ active: isInfo }"
                                                :to="{ path: 'info', query: { id: customerId } }">
                                                Thông tin chi tiết
                                            </router-link>
                                        </li>
                                        <li class="tab-item">
                                            <router-link class="tab-info" id="order" :class="{ active: isOrder }"
                                                :to="{ path: 'order', query: { id: customerId } }">
                                                Đơn đặt vé
                                            </router-link>
                                        </li>
                                        <li class="tab-item">
                                            <router-link class="tab-info" id="transfer" :class="{ active: isTransfer }"
                                                :to="{ path: 'transfer', query: { id: customerId } }">
                                                Đơn chuyển nhượng vé
                                            </router-link>
                                        </li>
                                        <li class="tab-item">
                                            <router-link class="tab-info" id="exchange" :class="{ active: isExchange }"
                                                :to="{ path: 'exchange', query: { id: customerId } }">
                                                Đơn Trả vé
                                            </router-link>
                                        </li>
                                        <li class="tab-item">
                                            <router-link class="tab-info" id="transfer-order"
                                                :class="{ active: isTransferOrder }"
                                                :to="{ path: 'transfer-order', query: { id: customerId } }">
                                                Vé được chuyển nhượng
                                            </router-link>
                                        </li>
                                    </ul>
                                </div>
                                <div class="col-md-4">

                                </div>
                            </div>
                        </template>
                        <div class="row" style="display: flex; justify-content: space-between;">
                            <div class="col-md-12">
                                <customer-info v-if="isInfo"></customer-info>
                                <order v-if="isOrder"></order>
                                <transfer v-if="isTransfer"></transfer>
                                <exchange v-if="isExchange"> </exchange>
                                <transfer-order v-if="isTransferOrder"></transfer-order>
                            </div>
                        </div>
                    </card>
                </div>
            </div>
        </div>
    </div>
</template>
    
<script>
import LTable from 'src/components/Table.vue'
import Card from 'src/components/Cards/Card.vue'
import axios from 'axios'
import moment from 'moment';
import helpService from 'src/service/help/helpService'
import CustomerInfo from './CustomerInfo/CustomerInfo.vue';
import Exchange from './CustomerInfo/Exchange.vue';
import Transfer from './CustomerInfo/Transfer.vue';
import Order from './CustomerInfo/Order.vue';
import TransferOrder from './CustomerInfo/TransferOrder.vue';
export default {
    components: {
        LTable,
        Card, CustomerInfo, Exchange, Transfer, Order, TransferOrder
    },
    data() {
        return {
            search: {
                keyword: null,
                status: null
            },
            isInfo: false,
            isTransfer: false,
            isExchange: false,
            isOrder: false,
            isTransferOrder: false,
            id: 0,
            customerId:0,
            isUpdate: false,
            pageSize: 100,
            pageNumber: 1,
            customers: [],
            imageUpload: null,
            fields: ['id',
                { key: 'firstName', label: 'Họ' },
                { key: 'lastName', label: 'Tên' },
                { key: 'address', label: 'Địa chỉ' },
                { key: 'country', label: 'Quốc gia' },
                { key: 'nationality', label: 'Quốc tịch' },
                { key: 'gender', label: 'Giới tính' },
                { key: 'dateOfBirth', label: 'Ngày sinh' },
                { key: 'action', label: 'Thao tác' },
            ],
            fieldAccounts: ['id',
                { key: 'username', label: 'Tên tài khoản' },
                { key: 'password', label: 'Mật khẩu' },
                { key: 'email', label: 'Email' },
                { key: 'phone', label: 'Số điện thoại' },
                { key: 'action', label: 'Thao tác' },
            ],
            updateAccount: {
                id: 0,
                username: null,
                password: null,
                email: null,
                phone: null
            },
            updateSupplier: {
                id: 0,
                fullName: null,
                shortName: null,
                address: null,
                taxCode: null,
                accounts: [
                    {
                        id: 0,
                        username: null,
                        email: null,
                        phone: null
                    }
                ]
            },
        };
    },
    methods: {
        async getCustomer() {
            try {
                const res = await axios.get(
                    "myticket/api/user/customer/find-all",
                    {
                        params: {
                            pageSize: this.pageSize,
                            pageNumber: this.pageNumber,
                            keyword: this.search.keyword,
                            status: this.search.status
                        },
                    }
                )
                return res.data.data.items;
            } catch (error) {
                console.error('API Error:', error);
                throw error;
            }
        },
        async getAllData() {
            try {
                this.customers = await this.getCustomer();
                this.customers.forEach(element => {
                    element.dateOfBirth = helpService.formatDate(element.dateOfBirth)
                });
            } catch (error) {
                console.error('Error fetching data:', error);
            }
        },
        async getSupplierById(id) {
            try {
                const res = await axios.get(
                    "myticket/api/user/supplier/find-by-id",
                    {
                        params: {
                            id: id,
                        },
                    }
                )
                return res.data.data;
            } catch (error) {
                console.error('API Error:', error);
                throw error;
            }
        },
        notifyVue(type, message, verticalAlign, horizontalAlign, color) {
            this.$notifications.notify(
                {
                    message: `<span><b>${type}</b> - ${message}</span>`,
                    icon: 'nc-icon nc-app',
                    horizontalAlign: horizontalAlign,
                    verticalAlign: verticalAlign,
                    type: color
                })
        },
        checkRouter() {
            this.isInfo = false;
            this.isOrder = false;
            this.isExchange = false;
            this.isTransfer = false;
            this.isTransferOrder= false;

            switch (true) {
                case this.$route.params.action === 'info':
                    this.isInfo = true;
                    break;
                case this.$route.params.action === 'order':
                    this.isOrder = true;
                    break;
                case this.$route.params.action === 'exchange':
                    this.isExchange = true;
                    break;
                case this.$route.params.action === 'transfer':
                    this.isTransfer = true;
                    break;
                case this.$route.params.action === 'transfer-order':
                    this.isTransferOrder = true;
                    break;
                default:
                    break;
            }
        }
    },
    mounted() {
        this.customerId = this.$route.query.id
        this.checkRouter();
        this.$nextTick(() => {
            console.log(this.isInfo);
        });
        this.getAllData();
    },
    computed: {
    },
    watch: {
        '$route.params': function (newVal, oldVal) {
            this.checkRouter();
        },
    },
};
</script>
    
<style>
.table-btn.btn {
    border: none !important;
}

.tab-info {
    display: block;
    padding: 10px 5px;
    color: black;
}

.tab-info:hover {
    border-bottom: 2px solid var(--primary-color-bold);
    color: var(--primary-color-bold);
}

.active {
    border-bottom: 2px solid var(--primary-color-bold);
    color: var(--primary-color-bold);
}
</style>
    