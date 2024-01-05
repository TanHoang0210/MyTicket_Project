<template>
    <div class="content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-12">
                    <card class="strpied-tabled-with-hover" body-classes="table-full-width table-responsive">
                        <template slot="header">
                            <div class="row" style="display: flex; justify-content: space-between;">
                                <div class="col-md-6">
                                    <h4 class="card-title">Thông báo hệ thống</h4>
                                </div>
                                <div class="col-md-4">
                                    <b-input-group class="float-right">
                                        <b-form-input v-model="search.keyword"></b-form-input>
                                        <b-input-group-append>
                                            <b-button class="btn-search" @click="getAllData()" variant="outline-success">Tìm
                                                kiếm</b-button>
                                        </b-input-group-append>
                                    </b-input-group>
                                </div>
                            </div>
                        </template>
                        <b-table striped hover :fields="fields" :items="notifications">
                            <template #cell(action)="data">
                                <div style="font-size: 1.2rem; !important">
                                    <b-button class="table-btn" variant="secondary"
                                        title="Xem chi tiết">
                                        <b-icon icon="pencil-square">
                                        </b-icon>
                                    </b-button>
                                </div>
                            </template>
                        </b-table>
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
import helpService from 'src/service/help/helpService'
export default {
    components: {
        LTable,
        Card
    },
    data() {
        return {
            search: {
                keyword: null,
                IsSeen: null
            },
            id: 0,
            isUpdate:false,
            pageSize: 100,
            pageNumber: 1,
            notifications: [],
            fields: ['id',
                { key: 'title', label: 'Thông báo' },
                { key: 'description', label: 'Nội dung'},
                { key: 'createDate', label: 'Thời gian' },
            ],
        };
    },
    methods: {
        async getNotification() {
            try {
                const res = await axios.get(
                    "myticket/api/notification/find-all",
                    {
                        params: {
                            pageSize: this.pageSize,
                            pageNumber: this.pageNumber,
                            keyword: this.search.keyword,
                            IsSeen: this.search.IsSeen
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
                this.notifications = await this.getNotification();
                this.notifications.forEach(element => {
                    element.createDate = helpService.formatDate(element.createDate)
                });
            } catch (error) {
                console.error('Error fetching data:', error);
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
        }
    },
    mounted() {
        this.getAllData();
    },
    computed: {
    }
};
</script>
    
<style>
.table-btn.btn {
    border: none !important;
}
</style>
    