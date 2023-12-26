<template>
    <div class="content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-12">
                    <card class="strpied-tabled-with-hover" body-classes="table-full-width table-responsive">
                        <template slot="header">
                            <div class="row" style="display: flex; justify-content: space-between;">
                                <div class="col-md-12">
                                    <h4 class="card-title">Thông tin chi tiết nhà cung cấp</h4>
                                    <ul style="display: flex;  list-style: none; margin-left: -40px;">
                                        <li class="tab-item">
                                            <router-link class="tab-info" id="info" :class="{ active: isInfo }"
                                                :to="{ path: 'info', query: { id: customerId } }">
                                                Thông tin chi tiết
                                            </router-link>
                                        </li>
                                        <li class="tab-item">
                                            <router-link class="tab-info" id="order" :class="{ active: isEvent }"
                                                :to="{ path: 'event', query: { id: customerId } }">
                                                Danh sách sự kiện
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
                                <supplier-info v-if="isInfo"></supplier-info>
                                <supplier-event v-if="isEvent"></supplier-event>
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
import SupplierInfo from './SupplierInfo/SupplierInfo.vue';
import SupplierEvent from './SupplierInfo/SupplierEvent.vue';
export default {
    components: {
        LTable,
        Card,SupplierEvent,SupplierInfo
    },
    data() {
        return {
            search: {
                keyword: null,
                status: null
            },
            isInfo: false,
            isEvent: false,
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
            this.isEvent = false;

            switch (true) {
                case this.$route.params.action === 'info':
                    this.isInfo = true;
                    break;
                case this.$route.params.action === 'event':
                    this.isEvent = true;
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
    