<template>
    <div class="content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-12">
                    <card class="strpied-tabled-with-hover" body-classes="table-full-width table-responsive">
                        <template slot="header">
                            <div class="row" style="display: flex; justify-content: space-between;">
                                <div class="col-md-6">
                                    <h4 class="card-title">Danh sách khách hàng</h4>
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
                        <div class="row">
                            <div class="col-md-10">

                            </div>
                            <div class="col-md-2">
                                <label for="per-page-select" style="text-align: end;">Số bản ghi</label>
                                <b-form-select style="width:50% right:0" id="per-page-select" v-model="pageSize"
                                    @change="getAllData()" :options="pageSizeOption" size="sm"></b-form-select>
                            </div>
                        </div>
                        <b-table :current-page="pageNumber" id="table-customer" striped hover :fields="fields"
                            :items="customers">
                            <template #cell(gender)="data">
                                <td v-if="data.item.gender === 1">Nam</td>
                                <td v-else-if="data.item.gender === 2">Nữ</td>
                                <td v-else>Khác</td>
                            </template>
                            <template #cell(action)="data">
                                <div style="font-size: 1.2rem; !important">
                                    <router-link class="btn btn-info" :style="{ border: 'none' }"
                                        :to="{ path: 'customer/info', query: { id: data.item.id } }">
                                        <b-icon icon="pencil-square">
                                        </b-icon>
                                    </router-link>
                                </div>
                            </template>
                        </b-table>
                        <b-pagination v-model="pageNumber" :total-rows="totals" :per-page="pageSize" aria-controls="table-customer"></b-pagination>
                    </card>
                    <b-modal id="modal-add-edit" ref="modal-add-edit" size="lg" title="Thông tin nhà cung cấp" ok-only>
                        <form>
                            <div class="row">
                                <div class="col-md-6">
                                    <base-input type="text" label="Tên đẩy đủ" placeholder="Nhập tên đầy đủ"
                                        v-model="updateSupplier.fullName">
                                    </base-input>
                                    <base-input type="text" label="Tên Doanh nghiệp" placeholder="Nhập tên nhà cung cấp"
                                        v-model="updateSupplier.shortName">
                                    </base-input>
                                </div>
                                <div class="col-md-6">
                                    <base-input type="text" label="Địa chỉ" placeholder="Nhập địa chỉ"
                                        v-model="updateSupplier.address">
                                    </base-input>
                                    <base-input type="text" label="Mã nhà cung cấp" placeholder="Nhập mã nhà cung cấp"
                                        v-model="updateSupplier.taxCode">
                                    </base-input>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <card v-if="updateSupplier.id !== 0">
                                        <b-table v-if="updateSupplier.accounts.length > 0" :fields="fieldAccounts"
                                            :items="updateSupplier.accounts">
                                            <template #cell(password)="data">
                                                ******
                                            </template>
                                            <template #cell(action)="data">
                                                <div style="font-size: 1.2rem; !important">
                                                    <b-button class="table-btn" variant="danger" title="Xóa">
                                                        <b-icon icon="trash">
                                                        </b-icon>
                                                    </b-button>
                                                    <b-button @click="showModalAccount(data.item.id)" class="table-btn"
                                                        variant="secondary" title="Xem chi tiết">
                                                        <b-icon icon="pencil-square">
                                                        </b-icon>
                                                    </b-button>
                                                </div>
                                            </template>
                                        </b-table>
                                        <button class="btn btn-success btn-fill float-right"
                                            style="color: #fff; margin-right: 30px;" @click="showModalAccount(0)">
                                            Thêm mới
                                        </button>
                                    </card>
                                </div>
                            </div>
                        </form>
                        <template #modal-footer="{ ok, cancel }">
                            <div style="margin: auto; width: 30%;">
                                <b-button class="buttonModal" size="lg" variant="success" @click="addEditSupplier()">
                                    OK
                                </b-button>
                                <b-button class="buttonModal" size="lg" variant="secondary" @click="cancel()">
                                    Cancel
                                </b-button>
                            </div>
                        </template>
                    </b-modal>
                    <b-modal centered id="modal-add-edit-account" ref="modal-add-edit-account" size="sm"
                        title="Thông tin tài khoản" ok-only>
                        <div class="row">
                            <div class="col-md-12">
                                <base-input type="text" label="Tên đăng nhập" placeholder="Nhập tên đăng nhập"
                                    v-model="updateAccount.username">
                                </base-input>
                                <base-input v-if="!isUpdate" type="text" label="Mật khẩu" placeholder="Nhập mật khẩu"
                                    v-model="updateAccount.password">
                                </base-input>
                                <base-input type="text" label="Địa chỉ Email" placeholder="Nhập địa chỉ email"
                                    v-model="updateAccount.email">
                                </base-input>
                                <base-input type="text" label="Số điện thoại" placeholder="Số điện thoại"
                                    v-model="updateAccount.phone">
                                </base-input>
                            </div>
                        </div>
                        <template #modal-footer="{ ok, cancel }">
                            <div style="margin: auto; width: 70%;">
                                <b-button class="buttonModal" size="lg" variant="success"
                                    @click="addEditSupplierAccount(updateSupplier.id)">
                                    OK
                                </b-button>
                                <b-button class="buttonModal" size="lg" variant="secondary" @click="cancel()">
                                    Cancel
                                </b-button>
                            </div>
                        </template>
                    </b-modal>
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
export default {
    components: {
        LTable,
        Card
    },
    data() {
        return {
            search: {
                keyword: null,
                status: null
            },
            pageSizeOption:[5,10,25,50,100],
            id: 0,
            isUpdate: false,
            pageSize: 100,
            pageNumber: 1,
            totals:0,
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
                this.totals = res.data.data.totalItems
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
                    element.dateOfBirth = helpService.formatDay(element.dateOfBirth)
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
        },
    },
    mounted() {
        this.getAllData();
    },
    computed: {
    },
    watch: {
    // Fetch data when the pageNumber changes
    pageNumber(newPage, oldPage) {
      if (newPage !== oldPage) {
        this.getAllData();
      }
    },
  },
};
</script>
    
<style>
.table-btn.btn {
    border: none !important;
}
</style>
    