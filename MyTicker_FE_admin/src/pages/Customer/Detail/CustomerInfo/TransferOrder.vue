<template>
    <div class="content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-12">
                    <card class="strpied-tabled-with-hover" body-classes="table-full-width table-responsive">
                        <template slot="header">
                            <div class="row" style="display: flex; justify-content: space-between;">
                                <div class="col-md-6">
                                    <h4 class="card-title">Danh sách vé khách hàng</h4>
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
                        <b-table striped hover :fields="fields" :items="orders">
                            <template #cell(status)="data">
                                <td style="color: blue;font-weight: 600;" v-if="data.item.status === 1">Khởi tạo</td>
                                <td style="color: greenyellow;font-weight: 600;" v-if="data.item.status === 2">Chưa thanh toán</td>
                                <td style="color: yellow;font-weight: 600;" v-if="data.item.status === 3">Đang thanh toán</td>
                                <td style="color: #888;font-weight: 600;" v-if="data.item.status === 4">Đã hủy</td>
                                <td style="color: orange;font-weight: 600;" v-if="data.item.status === 5">Đã thanh toán</td>
                                <td style="color: green;font-weight: 600;" v-if="data.item.status === 6">Đã nhận vé</td>
                            </template>
                            <template #cell(action)="data">
                                <div style="font-size: 1.2rem; !important">
                                    <b-button @click="showModalSupplier(data.item.id)" class="table-btn" variant="secondary"
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
                status: null
            },
            id: 0,
            isUpdate:false,
            pageSize: 100,
            pageNumber: 1,
            orders: [],
            imageUpload: null,
            fields: ['id',
                { key: 'orderCode', label: 'Mã đơn đặt vé ' },
                { key: 'orderDate', label: 'Ngày đặt vé' },
                { key: 'eventName', label: 'Tên sự kiện' },
                { key: 'ticketEventName', label: 'Hạng vé' },
                { key: 'status', label: 'Trạng thái' },
                { key: 'action', label: 'Thao tác' },
            ]
        };
    },
    methods: {
        async getOrder() {
            try {
                const res = await axios.get(
                    "myticket/api/order/admin/order-transfer/find-all",
                    {
                        params: {
                            customerId: this.$route.query.id,
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
        handleFileChange(event) {
            // Update the 'file' data property when the file input changes
            this.imageUpload = event.target.files[0];
        },
        async uploadImageType() {
            const formData = new FormData();
            formData.append('file', this.imageUpload);
            await axios.post('myticket/api/file/upload', formData).then(
                res => this.updateSupplier.image = res.data.data
            ).catch(
                err => err
            )
            console.log(this.updateSupplier.image)
        },
        async getAllData() {
            try {
                this.orders = await this.getOrder();
                this.orders.forEach(element => {
                    element.orderDate = helpService.formatDate(element.orderDate)
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
        async getSupplierAccountById(id) {
            try {
                const res = await axios.get(
                    "myticket/api/user/supplier/account/find-by-id",
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
        async showModalSupplier(id) {
            console.log(id)
            try {
                if (id !== 0) {
                    this.updateSupplier = await this.getSupplierById(id)
                } else {
                    this.updateSupplier.id = 0,
                        this.updateSupplier.fullName = null,
                        this.updateSupplier.shortName = null,
                        this.updateSupplier.address = null,
                        this.updateSupplier.taxCode = null,
                        this.updateSupplier.accounts = []
                }
                // this.venues = await this.findAllVenue();
                this.$nextTick(() => {
                    // Using $nextTick to ensure the modal component is updated
                    this.$refs['modal-add-edit'].show();
                });
            } catch (error) {
                console.error("Error fetching ticket details:", error);
            }
        },
        async showModalAccount(id) {
            try {
                if (id !== 0) {
                    this.updateAccount = await this.getSupplierAccountById(id)
                    this.isUpdate = true;
                } else {
                        this.isUpdate = false;
                        this.updateAccount.id = 0,
                        this.updateAccount.username = null,
                        this.updateAccount.password = null,
                        this.updateAccount.email = null,
                        this.updateAccount.phone = null
                }
                // this.venues = await this.findAllVenue();
                this.$nextTick(() => {
                    // Using $nextTick to ensure the modal component is updated
                    this.$refs['modal-add-edit-account'].show();
                });
            } catch (error) {
                console.error("Error fetching ticket details:", error);
            }
        },
        async addEditSupplier() {
            if (this.updateSupplier.id !== 0) {
                const response = await axios.put('myticket/api/user/update-by-suplier', {
                    fullName: this.updateSupplier.fullName,
                    shortName: this.updateSupplier.shortName,
                    address: this.updateSupplier.address,
                    taxCode: this.updateSupplier.taxCode,
                    id: this.updateSupplier.id
                }, {
                    headers: {
                        'Content-Type': 'application/json'
                    },
                })
                if (response.data.code === 200) {
                    this.notifyVue('Thành công', 'Cập nhật nhà cung cấp thành công', 'top', 'right', 'success')
                }
                else {
                    this.notifyVue('Thất bại', 'Cập nhật nhà cung cấp thất bại', 'top', 'right', 'danger')
                }
            } else {
                const response = await axios.post('myticket/api/user/add-by-suplier', {
                    fullName: this.updateSupplier.fullName,
                    shortName: this.updateSupplier.shortName,
                    address: this.updateSupplier.address,
                    taxCode: this.updateSupplier.taxCode,
                }, {
                    headers: {
                        'Content-Type': 'application/json'
                    },
                })
                if (response.data.code === 200) {
                    this.notifyVue('Thành công', 'Thêm nhà cung cấp thành công', 'top', 'right', 'success')
                }
                else {
                    this.notifyVue('Thất bại', 'Thêm nhà cung cấp thất bại', 'top', 'right', 'danger')
                }
            }
            this.$refs['modal-add-edit'].hide();
            this.getAllData();
        },
        async addEditSupplierAccount(supplierId) {
            if (this.updateAccount.id !== 0) {
                const response = await axios.put('myticket/api/user/account/update-by-suplier', {
                    username: this.updateAccount.username,
                    password: this.updateAccount.password,
                    email: this.updateAccount.email,
                    phone: this.updateAccount.phone,
                    id: this.updateAccount.id
                }, {
                    headers: {
                        'Content-Type': 'application/json'
                    },
                })
                if (response.data.code === 200) {
                    this.notifyVue('Thành công', 'Cập nhật tài khoản nhà cung cấp thành công', 'top', 'right', 'success')
                }
                else {
                    this.notifyVue('Thất bại', 'Cập nhật tài khoản nhà cung cấp thất bại', 'top', 'right', 'danger')
                }
            } else {
                const response = await axios.post('myticket/api/user/account/add-by-suplier', {
                    supplierId: supplierId,
                    username: this.updateAccount.username,
                    password: this.updateAccount.password,
                    phone: this.updateAccount.phone,
                    email: this.updateAccount.email,
                }, {
                    headers: {
                        'Content-Type': 'application/json'
                    },
                })
                if (response.data.code === 200) {
                    this.notifyVue('Thành công', 'Thêm tài khoản nhà cung cấp thành công', 'top', 'right', 'success')
                }
                else {
                    this.notifyVue('Thất bại', 'Thêm tài khoản nhà cung cấp thất bại', 'top', 'right', 'danger')
                }
            }
            this.$refs['modal-add-edit-account'].hide();
            this.$refs['modal-add-edit'].show();
        },
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
    