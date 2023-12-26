<template>
    <div class="content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-12">
                    <card class="strpied-tabled-with-hover" body-classes="table-full-width table-responsive">
                        <template slot="header">
                            <div class="row" style="display: flex; justify-content: space-between;">
                                <div class="col-md-6">
                                    <h4 class="card-title">Danh sách nhà cung cấp</h4>
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
                                <div class="col-md-2">
                                    <button class="btn btn-success btn-fill float-right"
                                        style="color: #fff; margin-right: 30px;" @click="showModalSupplier(0)">
                                        Thêm mới
                                    </button>
                                </div>
                            </div>
                        </template>
                        <b-table striped hover :fields="fields" :items="suppliers">
                            <template #cell(action)="data">
                                <div style="font-size: 1.2rem; !important">
                                    <router-link class="btn btn-info" :style="{ border:'none' }"
                                        :to="{path:'supplier/info',query:{id:data.item.id}}">
                                        <b-icon icon="pencil-square">
                                        </b-icon>
                                    </router-link>
                                </div>
                            </template>
                        </b-table>
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
                </div>
            </div>
        </div>
    </div>
</template>
    
<script>
import LTable from 'src/components/Table.vue'
import Card from 'src/components/Cards/Card.vue'
import axios from 'axios'
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
            suppliers: [],
            imageUpload: null,
            fields: ['id',
                { key: 'fullName', label: 'Tên đầy đủ doanh nghiệp ' },
                { key: 'shortName', label: 'Tên doanh nghiệp' },
                { key: 'address', label: 'Địa chỉ' },
                { key: 'taxCode', label: 'Mã số ' },
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
        async getSupplier() {
            try {
                const res = await axios.get(
                    "myticket/api/user/supplier/find-all",
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
                this.suppliers = await this.getSupplier();
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
    