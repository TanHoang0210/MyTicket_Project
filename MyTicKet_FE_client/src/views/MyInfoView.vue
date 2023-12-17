<template>
    <div>
        <div class="alert alert-section alert-grey mgt-16">
            <ul class="disc">
                <li>Vui lòng điền thông tin chính xác để đảm bảo quyền lợi mua sắm của bạn.</li>
                <li>Dữ liệu của bạn sẽ được sử dụng khi vé/hàng hóa được giao đến địa chỉ của bạn hoặc nhận tại địa điểm.
                    Vui lòng nhập chính xác tên thành viên, số điện thoại di động và địa chỉ người nhận.</li>
            </ul>
        </div>
        <div class="card">
            <div class="card-header" id="my-account-title">Thông tin tài khoản</div>
            <div id="my-account" class="" aria-labelledby="my-account" data-parent="#myProfile">
                <div class="card-body">
                    <div class=" row mb-3">
                        <div class="col-lg-3 col-md-3 col-sm-4 col-xs-12 col-12 control-label text-sm-end email"><label
                                class="col-form-label required">Email</label></div>
                        <div class="col-lg-7 col-md-9 col-sm-8 col-xs-12 col-12 input-field email">{{ currentUser.email }}
                        </div>
                    </div>
                    <div class=" row mb-3">
                        <div class="col-lg-3 col-md-3 col-sm-4 col-xs-12 col-12 control-label text-sm-end phone"><label
                                class="col-form-label required">Số điện thoại</label></div>
                        <div class="col-lg-7 col-md-9 col-sm-8 col-xs-12 col-12 input-field email">{{ currentUser.phone }}
                        </div>
                    </div>
                    <div class=" row mb-3">
                        <div class="col-lg-3 col-md-3 col-sm-4 col-xs-12 col-12 control-label text-sm-end phone"><label
                                class="col-form-label required">Tên đăng nhập</label></div>
                        <div class="col-lg-7 col-md-9 col-sm-8 col-xs-12 col-12 input-field email">{{ currentUser.username
                        }}
                        </div>
                    </div>
                    <div class=" row mb-3">
                        <div class="col-lg-3 col-md-3 col-sm-4 col-xs-12 col-12 control-label text-sm-end password"><label
                                class="col-form-label required">Password</label></div>
                        <div class="col-lg-7 col-md-9 col-sm-8 col-xs-12 col-12 input-field password"><span type="password"
                                style="vertical-align: middle;">**********</span>
                            <button type="button" @click="showModalChangePassword()" id="resetButton"
                                class="btn btn-primary btn-sm mx-3">
                                Đổi mật khẩu
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <b-card bg-variant="light">
            <div class="card-header" id="my-account-title">Thông tin khách hàng</div>
            <b-form @submit="updateCustomer" @reset="onReset">
                <b-form-group style="width: 50%;margin: auto;">
                    <b-form-group label="Họ:" label-for="nested-street" label-cols-sm="3" label-align-sm="">
                        <b-form-input required v-model="userUpdate.firstName" id="nested-street"></b-form-input>
                    </b-form-group>

                    <b-form-group label="Tên:" label-for="nested-city" label-cols-sm="3" label-align-sm="">
                        <b-form-input required v-model="userUpdate.lastName" id="nested-city"></b-form-input>
                    </b-form-group>

                    <b-form-group label="Địa chỉ:" label-for="nested-state" label-cols-sm="3" label-align-sm="">
                        <b-form-input v-model="userUpdate.address" id="nested-state"></b-form-input>
                    </b-form-group>

                    <b-form-group label="Ngày Sinh:" label-for="nested-country" label-cols-sm="3" label-align-sm="">
                        <b-form-datepicker v-model="userUpdate.dateOfBirth"
                            :date-format-options="{ year: 'numeric', month: 'numeric', day: 'numeric' }"
                            locale="en"></b-form-datepicker>
                    </b-form-group>

                    <b-form-group label="Quốc tịch:" label-for="nested-country" label-cols-sm="3" label-align-sm="">
                        <b-form-select v-model="userUpdate.country">
                            <b-form-select-option v-for="country in countries" :key="country.name.common"
                                :value="country.name.common">
                                <span class="fi" :style="{ 'background-image': 'url(' + country.flags.png + ')' }"></span>
                                {{ country.name.common }}
                            </b-form-select-option>
                        </b-form-select>
                    </b-form-group>

                    <b-form-group label="Quốc gia:" label-for="nested-country" label-cols-sm="3" label-align-sm="">
                        <b-form-select v-model="userUpdate.nationality">
                            <b-form-select-option v-for="country in countries" :key="country.name.common"
                                :value="country.name.common">
                                <span class="fi" :style="{ 'background-image': 'url(' + country.flags.png + ')' }"></span>
                                {{ country.name.common }}
                            </b-form-select-option>
                        </b-form-select>

                    </b-form-group>
                    <b-form-group label="Giới tính:" label-cols-sm="3" v-slot="{ ariaDescribedby }">
                        <b-form-radio-group class="pt-2" v-model="userUpdate.gender" :aria-describedby="ariaDescribedby">
                            <b-form-radio :value="1">Nam</b-form-radio>
                            <b-form-radio :value="2">Nữ</b-form-radio>
                            <b-form-radio :value="3">Khác</b-form-radio>
                        </b-form-radio-group>
                    </b-form-group>
                    <b-form-group label-cols-sm="3" label-align-sm="right">
                        <b-button type="submit" variant="primary">Cập nhật</b-button>
                        <b-button type="reset" variant="secondary">Hủy</b-button>
                    </b-form-group>
                </b-form-group>
            </b-form>
        </b-card>
        <b-modal ref="modal-changePassword" centered title="Xác nhận đổi mật khẩu">
            <h4 class="my-4">Nhập thông tin để đổi mật khẩu?</h4>
            <template #modal-footer="{ ok, cancel }">
                <b-form-group style="width: 100%;" label="Mật khẩu cũ:" label-for="oldPass">
                    <b-form-input type="password" required v-model="changePass.oldPass" id="oldPass"></b-form-input>
                </b-form-group>
                <b-form-group style="width: 100%;" label="Mật khẩu mới:" label-for="newPass">
                    <b-form-input type="password" required v-model="changePass.newPass" id="newPass"></b-form-input>
                </b-form-group>
                <b-form-group style="width: 100%;" label="Nhập lại mật khẩu mới:" label-for="confirmNewPass">
                    <b-form-input type="password" required v-model="changePass.confirmNewPass"
                        id="confirmNewPass"></b-form-input>
                </b-form-group>
                <b-button size="lg" variant="success" @click="changePassword()">
                    OK
                </b-button>
                <b-button size="lg" variant="secondary" @click="cancel()">
                    Cancel
                </b-button>
            </template>
        </b-modal>
    </div>
</template>

<script>
import axios from 'axios';
import store from '@/store';
import { getCurrentUser } from '@/service/auth/authService'
export default {
    data() {
        return {
            changePass: {
                oldPass: null,
                newPass: null,
                confirmNewPass: null,
            },
            userUpdate: {
                firstName: null,
                lastName: null,
                country: null,
                nationality: null,
                address: null,
                gender: null,
                dateOfBirth: null
            },
            countries: [],
        }
    },
    computed: {
        currentUser() {
            return this.$store.getters.currentUser;
        }
    },
    methods: {
        async logout() {
                const response = await axios.post('connect/logout', {}, {
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded',
                    }
                })
                this.$toasted.success("Vui lòng đăng nhập lại bằng mật khẩu mới", {
                    position: 'top-center',
                    duration: 3000, // Thời gian hiển thị toast (ms)
                    theme: 'outline', // Theme: 'outline', 'bubble'
                    iconPack: 'fontawesome', // Icon pack: 'fontawesome', 'mdi'
                    icon: 'check', // Tên icon, ví dụ: 'check' (cho fontawesome)
                    iconColor: 'white', // Màu của icon
                    containerClass: 'custom-toast-container-class', // Thêm class cho container
                    singleton: true, // Hiển thị toast duy nhất, không hiển thị toast mới nếu toast trước chưa biến mất
                });
                store.dispatch('logout');
                this.$router.push('/login');
        },
        async getAllCountry() {
            try {
                const res = await axios.get(
                    "https://restcountries.com/v3.1/all"
                );
                return res.data;
            } catch (error) {
                console.error('Lỗi API 1:', error);
                throw error;
            }
        },
        showModalChangePassword() {
            this.$nextTick(() => {
                // Using $nextTick to ensure the modal component is updated
                this.$refs['modal-changePassword'].show();
            });
        },
        async changePassword() {
            if (this.changePass.newPass !== this.changePass.confirmNewPass) {
                this.$toasted.error("Mật khẩu mới không đúng vui lòng thử lại!", {
                    position: 'top-right',
                    duration: 3000, // Thời gian hiển thị toast (ms)
                    theme: 'outline', // Theme: 'outline', 'bubble'
                    iconPack: 'fontawesome', // Icon pack: 'fontawesome', 'mdi'
                    icon: 'time', // Tên icon, ví dụ: 'check' (cho fontawesome)
                    iconColor: 'white', // Màu của icon
                    containerClass: 'custom-toast-container-class', // Thêm class cho container
                    singleton: true, // Hiển thị toast duy nhất, không hiển thị toast mới nếu toast trước chưa biến mất
                });
            }
            else {
                try {
                    const res = await axios.put('myticket/api/user/change-password',
                        {
                            oldPassword: this.changePass.oldPass,
                            newPassword: this.changePass.newPass,
                        })
                    if (res.data.code !== 200) {
                        this.$toasted.error("Thay đổi mật khẩu thất bại", {
                            position: 'top-right',
                            duration: 3000, // Thời gian hiển thị toast (ms)
                            theme: 'outline', // Theme: 'outline', 'bubble'
                            iconPack: 'fontawesome', // Icon pack: 'fontawesome', 'mdi'
                            icon: 'time', // Tên icon, ví dụ: 'check' (cho fontawesome)
                            iconColor: 'white', // Màu của icon
                            containerClass: 'custom-toast-container-class', // Thêm class cho container
                            singleton: true, // Hiển thị toast duy nhất, không hiển thị toast mới nếu toast trước chưa biến mất
                        });
                    } else {
                        this.$toasted.success("Thay đổi mật khẩu thành công", {
                            position: 'top-right',
                            duration: 3000, // Thời gian hiển thị toast (ms)
                            theme: 'outline', // Theme: 'outline', 'bubble'
                            iconPack: 'fontawesome', // Icon pack: 'fontawesome', 'mdi'
                            icon: 'time', // Tên icon, ví dụ: 'check' (cho fontawesome)
                            iconColor: 'white', // Màu của icon
                            containerClass: 'custom-toast-container-class', // Thêm class cho container
                            singleton: true, // Hiển thị toast duy nhất, không hiển thị toast mới nếu toast trước chưa biến mất
                        });
                        this.$refs['modal-changePassword'].hide();
                        await this.logout()
                    }
                }
                catch (error) {
                    this.$toasted.error(error.response.data.error_description, {
                        position: 'top-right',
                        duration: 3000, // Thời gian hiển thị toast (ms)
                        theme: 'outline', // Theme: 'outline', 'bubble'
                        iconPack: 'fontawesome', // Icon pack: 'fontawesome', 'mdi'
                        icon: 'time', // Tên icon, ví dụ: 'check' (cho fontawesome)
                        iconColor: 'white', // Màu của icon
                        containerClass: 'custom-toast-container-class', // Thêm class cho container
                        singleton: true, // Hiển thị toast duy nhất, không hiển thị toast mới nếu toast trước chưa biến mất
                    });
                }
            }
        },
        async updateCustomer(event) {
            event.preventDefault()
            console.log(this.userUpdate)
            if (this.userUpdate.lastName == this.currentUser.lastName
                && this.userUpdate.firstName == this.currentUser.firstName
                && this.userUpdate.address == this.currentUser.address
                && this.userUpdate.dateOfBirth == this.currentUser.dateOfBirth
                && this.userUpdate.country == this.currentUser.country
                && this.userUpdate.nationality == this.currentUser.nationality
                && this.userUpdate.gender == this.currentUser.gender) {
                this.$toasted.info("Không có thông tin nào để cập nhật!", {
                    position: 'top-right',
                    duration: 3000, // Thời gian hiển thị toast (ms)
                    theme: 'outline', // Theme: 'outline', 'bubble'
                    iconPack: 'fontawesome', // Icon pack: 'fontawesome', 'mdi'
                    icon: 'time', // Tên icon, ví dụ: 'check' (cho fontawesome)
                    iconColor: 'white', // Màu của icon
                    containerClass: 'custom-toast-container-class', // Thêm class cho container
                    singleton: true, // Hiển thị toast duy nhất, không hiển thị toast mới nếu toast trước chưa biến mất
                });
            } else {
                try {
                    await axios.put('myticket/api/user/customer/update',
                        {
                            lastName: this.userUpdate.lastName,
                            firstName: this.userUpdate.firstName,
                            address: this.userUpdate.address,
                            dateOfBirth: this.userUpdate.dateOfBirth,
                            country: this.userUpdate.country,
                            nationality: this.userUpdate.nationality,
                            gender: this.userUpdate.gender
                        })
                    this.$toasted.success("Thay đổi thông tin thành công", {
                        position: 'top-right',
                        duration: 3000, // Thời gian hiển thị toast (ms)
                        theme: 'outline', // Theme: 'outline', 'bubble'
                        iconPack: 'fontawesome', // Icon pack: 'fontawesome', 'mdi'
                        icon: 'time', // Tên icon, ví dụ: 'check' (cho fontawesome)
                        iconColor: 'white', // Màu của icon
                        containerClass: 'custom-toast-container-class', // Thêm class cho container
                        singleton: true, // Hiển thị toast duy nhất, không hiển thị toast mới nếu toast trước chưa biến mất
                    });
                    await getCurrentUser()
                    sessionStorage.setItem("currentUser", JSON.stringify(store.getters.currentUser))
                    this.$router.push('/');
                }
                catch (error) {
                    this.$toasted.error(error.response.data.error_description, {
                        position: 'top-right',
                        duration: 3000, // Thời gian hiển thị toast (ms)
                        theme: 'outline', // Theme: 'outline', 'bubble'
                        iconPack: 'fontawesome', // Icon pack: 'fontawesome', 'mdi'
                        icon: 'time', // Tên icon, ví dụ: 'check' (cho fontawesome)
                        iconColor: 'white', // Màu của icon
                        containerClass: 'custom-toast-container-class', // Thêm class cho container
                        singleton: true, // Hiển thị toast duy nhất, không hiển thị toast mới nếu toast trước chưa biến mất
                    });
                }
            }
        },
        onReset(event) {
            event.preventDefault()
            this.userUpdate.dateOfBirth = this.currentUser.dateOfBirth
            this.userUpdate.gender = this.currentUser.gender
            this.userUpdate.address = this.currentUser.address
            this.userUpdate.nationality = this.currentUser.nationality
            this.userUpdate.country = this.currentUser.country
            this.userUpdate.lastName = this.currentUser.lastName
            this.userUpdate.firstName = this.currentUser.firstName
        },
        async fetchData() {
            this.countries = await this.getAllCountry();
            console.log(this.countries);
        }
    },
    watch: {
        countries(newCountries) {
            console.log(newCountries);
        }
    },
    mounted() {
        this.fetchData();
        this.userUpdate.dateOfBirth = this.currentUser.dateOfBirth
        this.userUpdate.gender = this.currentUser.gender
        this.userUpdate.address = this.currentUser.address
        this.userUpdate.nationality = this.currentUser.nationality
        this.userUpdate.country = this.currentUser.country
        this.userUpdate.lastName = this.currentUser.lastName
        this.userUpdate.firstName = this.currentUser.firstName
    }
}
</script>

<style>
#my-account {
    margin: auto;
    width: 50%;
}

.fi {
    background-size: contain;
    background-position: 50%;
    background-repeat: no-repeat;
    position: relative;
    display: inline-block;
    width: 1.33333333em;
    line-height: 1em;
}
</style>