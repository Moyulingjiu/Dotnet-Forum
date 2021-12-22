<template>
	<view>
		<view class="logo_container">
			<image class="logo" src="../../static/logo.png" mode="aspectFill"></image>
		</view>
		<view class="name_container">
			<text class="name">XMU校园论坛</text>
		</view>
		<view class="login_box">
			<view class="input_box">
				<input maxlength=30 type="text" placeholder="请输入学号" v-model="studentId" />
			</view>
			<view class="input_box flex_box">
				<input maxlength=20 type="text" password="true" placeholder="请输入密码" v-model="password" />
				<image class="cancel_icon" v-if="password.length!=0" src="../../static/cancel.png"
					@click="clearPassword()"></image>
			</view>
			<button class="login_button" @click="login()">登 陆</button>
			<button class="login_button register" @click="register()">注 册</button>
		</view>
	</view>
</template>

<script>
	import * as userApi from "../../api/user.js"
	import * as config from "../../utils/config.js"

	export default {
		data() {
			return {
				studentId: '',
				password: ''
			}
		},
		onLoad() {
			this.refresh()
		},
		onShow() {
			this.refresh()
		},
		methods: {
			refresh() {
				if (config.checkToken()) {
					uni.reLaunch({
						url: '../index/index'
					})
				}
			},
			clearPassword() {
				this.password = ''
			},
			login() {
				if (this.studentId.length === 0 || this.password.length === 0) {
					uni.showToast({
						title: '请填写学号和密码',
						icon: 'error',
						duration: 2000,
						position: 'top'
					});
					return
				}
				uni.showLoading({
					title: 'loading'
				})
				userApi.login(this.studentId, this.password).then(data => {
					let result = true
					if (typeof data === "undefined") {
						result = false
					} else {
						let token = data.data.token
						if (config.saveToken(token)) {
							uni.showToast({
								title: '登陆成功',
								icon: "success",
								mask: true,
								duration: 2000
							})
							setTimeout(() => {
								uni.reLaunch({
									url: '../index/index'
								})
							}, config.waitTime)
						} else {
							result = false
						}
					}
					if (!result) {
						uni.showToast({
							title: '学号或密码错误',
							icon: "error",
							mask: true,
							duration: 2000
						});
					}
				})
				uni.hideLoading()
			},
			register() {
				uni.redirectTo({
					url: '/pages/register/register'
				});
			}
		}
	}
</script>

<style>
	.flex_box {
		display: flex;
	}

	.logo_container {
		margin: auto;
		margin-top: 200rpx;
		width: 90%;
		height: 300rpx;
		text-align: center;
	}

	.logo {
		width: 300rpx;
		height: 100%;
	}

	.name_container {
		margin-top: 40rpx;
		text-align: center;
	}

	.name {
		font-size: 50rpx;
		font-weight: bold;
	}

	.login_box {
		margin: auto;
		margin-top: 75rpx;
		width: 90%;
	}

	.input_box {
		margin: 0rpx 10rpx 30rpx 10rpx;
		padding: 10rpx;
		padding-left: 40rpx;
		height: 60rpx;
		border-width: 3rpx;
		border-style: solid;
		border-color: #A0A0A0;
		border-radius: 40rpx;
	}

	.input_box input {
		font-size: 40rpx;
		width: 92%;
	}

	.cancel_icon {
		width: 30rpx;
		height: 30rpx;
		margin-top: 15rpx;
	}

	.login_button {
		margin: 30rpx 10rpx;
		background-image: linear-gradient(120deg, #a1c4fd 0%, #c2e9fb 100%);
		font-weight: bold;
		border-radius: 40rpx;
		box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
	}

	.register {
		border-width: 3rpx;
		border-style: solid;
		border-color: #A0A0A0;
		background-image: none;
		background-color: #FFF;
	}
</style>
