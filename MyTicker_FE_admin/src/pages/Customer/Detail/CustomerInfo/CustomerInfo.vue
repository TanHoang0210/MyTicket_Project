<template>
    <div class="content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <card>
                        <div>
                        <div class="row">
                            <h4 slot="header" class="card-title col-md-6">Thông tin chung </h4>
                        </div>
                        <form>
                            <div class="row">
                                <div class="col-md-3">
                                    <base-input type="text" label="Tên đăng nhập" :disabled="!isUpdate"
                                        placeholder="Tên đăng nhập" v-model="customerInfo.username">
                                    </base-input>
                                </div>
                                <div class="col-md-6">
                                    <base-input type="text" label="Email" :disabled="!isUpdate"
                                        placeholder="Giới tính" v-model="customerInfo.email">
                                    </base-input>
                                </div>
                                <div class="col-md-3">
                                    <base-input type="text" label="Số điện thoại" :disabled="!isUpdate"
                                        placeholder="Ngày sinh" v-model="customerInfo.phone">
                                    </base-input>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <base-input type="text" label="Họ khách hàng" :disabled="!isUpdate"
                                        placeholder="Họ khách hàng" v-model="customerInfo.firstName">
                                    </base-input>
                                </div>
                                <div class="col-md-3">
                                    <base-input type="text" label="Tên khách hàng" :disabled="!isUpdate"
                                        placeholder="Tên khách hàng" v-model="customerInfo.lastName">
                                    </base-input>
                                </div>
                                <div class="col-md-3">
                                    <base-input type="text" label="Giới tính" :disabled="!isUpdate"
                                        placeholder="Giới tính" v-model="customerInfo.gender">
                                    </base-input>
                                </div>
                                <div class="col-md-3">
                                    <base-input type="text" label="Ngày sinh" :disabled="!isUpdate"
                                        placeholder="Ngày sinh" v-model="customerInfo.dateOfBirth">
                                    </base-input>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <base-input type="text" label="Địa chỉ" :disabled="!isUpdate"
                                        placeholder="Địa chỉ" v-model="customerInfo.address">
                                    </base-input>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <base-input type="text" label="Quốc gia" :disabled="!isUpdate"
                                        placeholder="Quốc gia" v-model="customerInfo.country">
                                    </base-input>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <base-input type="text" label="Quốc tịch" :disabled="!isUpdate"
                                        placeholder="Quốc tịch" v-model="customerInfo.nationality">
                                    </base-input>
                                    </div>
                                </div>
                            </div>
                        </form>
                    </div>
                    </card>
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
            customerInfo: {
                id: 0,
                username: null,
                password: null,
                email: null,
                phone: null,
                firstName: null,
                lastName: null,
                country: null,
                nationality: null,
                address: null,
                gender: null,
                dateOfBirth: null
            },
        }
    },
    methods: {
        async getCustomerById() {
            try {
                const res = await axios.get(
                    "myticket/api/user/customer/find-by-id", {
                    params: {
                        id: this.$route.query.id
                    }
                })
                return res.data.data
            } catch (error) {
                console.error('API 1 Error:', error);
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
                this.customerInfo = await this.getCustomerById()
                this.customerInfo.dateOfBirth = helpService.formatDay(this.customerInfo.dateOfBirth)
                if(this.customerInfo.gender === 1){
                    this.customerInfo.gender = "Nam"
                }else if(this.customerInfo.gender === 2){
                    this.customerInfo.gender = "Nữ"
                }else {
                    this.customerInfo.gender = "Khác"
                }
            } catch (error) {
                console.error('API 1 Error:', error);
                throw error;
            }
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
  