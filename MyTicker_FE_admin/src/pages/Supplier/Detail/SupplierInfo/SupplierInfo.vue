<template>
    <div class="content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <card>
                        <div class="row">
                            <h4 slot="header" class="card-title col-md-6">Thông tin chung </h4>
                        </div>
                        <form>
                            <div class="row">
                                <div class="col-md-9">
                                    <base-input type="text" label="Tên đầy đủ" :disabled="!isUpdate"
                                        placeholder="Tên đầy đủ" v-model="supplierInfo.fullName">
                                    </base-input>
                                </div>
                                <div class="col-md-3">
                                    <base-input type="text" label="Tên rút gọn" :disabled="!isUpdate" placeholder="tên rút gọn"
                                        v-model="supplierInfo.shortName">
                                    </base-input>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-9">
                                    <base-input type="text" label="Địa chỉ" :disabled="!isUpdate"
                                        placeholder="Địa chỉ" v-model="supplierInfo.address">
                                    </base-input>
                                </div>
                                <div class="col-md-3">
                                    <base-input type="text" label="Mã số" :disabled="!isUpdate"
                                        placeholder="Mã số" v-model="supplierInfo.taxCode">
                                    </base-input>
                                </div>
                            </div>
                        </form>
                        <div>
                            <h4 slot="header" class="card-title col-md-6">Danh sách tài khoản</h4>
                            <button class="btn btn-success btn-fill float-right"
                                        style="color: #fff; margin-right: 30px;" @click="showModalAccount(0)">
                                        Thêm mới
                                    </button>
                            <b-table striped hover :fields="fieldAccounts" :items="supplierInfo.accounts">
                                <template #cell(password)>
                                    ******
                                </template>
                                <template #cell(action)="data">
                                    <div style="font-size: 1.2rem; !important">
                                        <b-button @click="showModalAccount(data.item.id)" title="Chỉnh sửa" class="btn btn-info" :style="{ border:'none' }">
                                            <b-icon icon="pencil-square">
                                            </b-icon>
                                        </b-button>
                                        <b-button @click="showModalAccount(data.item.id)" title="Đổi mật khẩu" class="btn btn-warning" :style="{ border:'none' }">
                                            <b-icon icon="recycle">
                                            </b-icon>
                                        </b-button>
                                    </div>
                                </template>
                            </b-table>
                        </div>
                    </card>
                    <b-modal centered id="modal-add-edit-account" ref="modal-add-edit-account" size="lg"
                        title="Thông tin tài khoản" ok-only>
                        <div class="row">
                            <div class="col-md-6">
                                <base-input type="text" label="Tên đăng nhập" placeholder="Nhập tên đăng nhập"
                                    v-model="updateAccount.username">
                                </base-input>
                                <base-input v-if="!isUpdate" type="text" label="Mật khẩu" placeholder="Nhập mật khẩu"
                                    v-model="updateAccount.password">
                                </base-input>
                                </div>
                                <div class="col-md-6">
                                <base-input type="text" label="Địa chỉ Email" placeholder="Nhập địa chỉ email"
                                    v-model="updateAccount.email">
                                </base-input>
                                <base-input type="text" label="Số điện thoại" placeholder="Số điện thoại"
                                    v-model="updateAccount.phone">
                                </base-input>
                            </div>
                        </div>
                        <template #modal-footer="{ ok, cancel }">
                            <div style="margin: auto;">
                                <b-button class="buttonModal" size="lg" variant="success" @click="addEditSupplierAccount(supplierInfo.id)">
                                    <span v-if="updateAccount.id === 0">Thêm mới</span>
                                    <span v-else>Cập nhật</span>
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
import FlatPickr from 'vue-flatpickr-component';
import 'flatpickr/dist/flatpickr.css';
import Card from 'src/components/Cards/Card.vue'
import axios from 'axios'
import helpService from 'src/service/help/helpService'
import moment from 'moment'
export default {
    components: {
        Card, FlatPickr
    },
    data() {
        return {
            dateTimePickerConfig: {
                enableTime: true,
                dateFormat: 'H:i d/m/Y',
            },
            supplierInfo: {
                id: 0,
                fullName: "string",
                shortName: "string",
                address: "string",
                taxCode: "string",
                accounts: [
                    {
                        id: 0,
                        username: "string",
                        password: "string",
                        email: "string",
                        phone: "string"
                    }
                ]
            },
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
        }
    },
    methods: {
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
        handleFileChange(event) {
            // Update the 'file' data property when the file input changes
            this.seatImage = event.target.files[0];
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
        async fetchData() {
            try {
                this.supplierInfo = await this.getSupplierById(this.$route.query.id)
            } catch (error) {
                console.error('API 1 Error:', error);
                throw error;
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
            this.fetchData();
        },
    }, mounted() {
        this.fetchData();
        console.log(this.listType)
    },


}

</script>
<style>
.select-form {
    width: 100%;
    height: 40px;
    border: 1px solid #E3E3E3;
    border-radius: 4px;
    padding: 8px 12px;
    color: #565656;
    font-size: 1rem;
}

.buttonModal {
    margin: 0 5px !important;
}

.btn-img {
    margin: 0 5px;
}

.modal-header {
    background-color: var(--primary-color-bold) !important;
}

.modal-title {
    font-size: 1.6rem !important;
}
</style>
  