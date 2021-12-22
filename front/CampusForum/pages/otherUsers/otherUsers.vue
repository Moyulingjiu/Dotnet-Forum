<template>
	<view>
		<view class="person_card">
			<view class="flex_box">
				<image class="person_avater" :src="user.avater" mode="aspectFill"></image>
				<view class="person_information">
					<text class="person_name">{{ user.name }}</text>
					<view>
						<image class="subscribe_icon" :src="isSubscribe?subscribeIcon:unsubscribedIcon" @click="like()"
							mode='aspectFit'></image>
					</view>
					<view class="person_description">
						<text>{{ user.description }}</text>
					</view>
					<text class="person_follow">关注：</text>
					<text class="person_follow">{{ user.following }}</text>
					<text class="person_follow inner_split">粉丝：</text>
					<text class="person_follow">{{ user.follower }}</text>
				</view>
			</view>
		</view>
		<view class="person_card">
			<view class="information_card flex_box">
				<view class="information_container">
					<view class="information_title">
						<text>学号</text>
					</view>
					<view class="information_text">
						<text>{{ user.studentId }}</text>
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view class="information_container">
					<view class="information_title">
						<text>学院</text>
					</view>
					<view class="information_text">
						<text>{{ user.collegeName }}</text>
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view class="information_container">
					<view class="information_title">
						<text>性别</text>
					</view>
					<view class="information_text">
						<text>{{ getGender() }}</text>
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view class="information_container">
					<view class="information_title">
						<text>电话</text>
					</view>
					<view class="information_text">
						<text>{{ user.phone }}</text>
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view class="information_container">
					<view class="information_title">
						<text>邮箱</text>
					</view>
					<view class="information_text">
						<text>{{ user.email }}</text>
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view class="information_container">
					<view class="information_title">
						<text>生日</text>
					</view>
					<view class="information_text">
						<text>{{ user.birthday }}</text>
					</view>
				</view>
			</view>
		</view>


		<view class="bottom_tips">
			<text>{{ '- 到底了 -' }}</text>
		</view>

		<uni-popup ref="popup_success" type="message">
			<uni-popup-message type="success" message="登出成功" :duration="3000"></uni-popup-message>
		</uni-popup>
	</view>
</template>

<script>
	import * as config from "../../utils/config.js"
	import * as userApi from "../../api/user.js"

	export default {
		data() {
			return {
				subscribeIcon: '../../static/like_active.png', // 点赞图标
				unsubscribedIcon: '../../static/like.png', // 未点赞图标
				isSubscribe: false,
				id: 0,
				user: {
					studentId: 10000000001,
					name: '晨曦氤氲',
					collegeName: '神秘学院',
					gender: 1,
					avater: '../../static/avater.jpg',
					description: '落花风雨更伤春，不如怜取眼前人。',
					birthday: '2021-12-20',
					phone: '12312341234',
					email: '123456@outlook.com',
					following: 100,
					follower: 100
				}
			}
		},
		onLoad(options) {
			this.id = options.id
		},
		onShow() {
			this.refresh()
		},
		methods: {
			refresh() {
				if (!config.checkToken()) {
					uni.redirectTo({
						url: '../login/login'
					})
				} else if (this.id == 0 || typeof this.id === "undefined") {
					uni.switchTab({
						url: '../index/index'
					})
				} else {
					userApi.select(this.id).then(data => {
						if (typeof data === "undefined") {
							uni.showToast({
								title: "服务器错误",
								icon: "error",
								mask: true,
								duration: 2000
							})
						} else if (data.code != 200) {
							uni.showToast({
								title: data.msg,
								icon: "error",
								mask: true,
								duration: 2000
							})
						} else {
							console.log(data)
							this.user.studentId = data.data.student_id
							this.user.name = data.data.name
							this.user.collegeName = data.data.college
							this.user.gender = data.data.gender
							this.user.avater = data.data.avater
							this.user.description = data.data.description
							this.user.birthday = data.data.birthday
							this.user.phone = data.data.phone
							this.user.email = data.data.email
							this.user.follower = data.data.follower
							this.user.following = data.data.following
						}
					})
				}
			},
			getGender() {
				return config.getGender(this.user.gender)
			},
			like() {
				if (this.isSubscribe) {
					userApi.unfollow(this.id).then(data => {
						if (typeof data === "undefined") {
							uni.showToast({
								title: "服务器错误",
								icon: "error",
								mask: true,
								duration: 2000
							})
						} else if (data.code != 200) {
							uni.showToast({
								title: data.msg,
								icon: "error",
								mask: true,
								duration: 2000
							})
						} else {
							if (data.data) {
								this.user.isSubscribe = false
								this.refresh()
							}
						}
					})
				} else {
					userApi.follow(this.id).then(data => {
						if (typeof data === "undefined") {
							uni.showToast({
								title: "服务器错误",
								icon: "error",
								mask: true,
								duration: 2000
							})
						} else if (data.code != 200) {
							uni.showToast({
								title: data.msg,
								icon: "error",
								mask: true,
								duration: 2000
							})
						} else {
							if (data.data) {
								this.user.isSubscribe = true
								this.refresh()
							}
						}
					})
				}
			},
		}
	}
</script>

<style>
	.flex_box {
		display: flex;
	}

	.inner_split {
		margin-left: 20rpx;
	}

	.person_card {
		width: 90%;
		margin-left: 5%;
		margin-right: 5%;
		margin-top: 40rpx;
		/* height: 160rpx; */
		background-color: #F0F0F0;
		padding-left: 10rpx;
		padding-top: 10rpx;
		padding-bottom: 10rpx;
		border-radius: 10rpx;
		box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
	}

	.person_avater {
		width: 150rpx;
		height: 150rpx;
		border-radius: 50%;
		margin-right: 20rpx;
	}

	.person_information {
		/* margin-top: 10rpx; */
		margin-left: 10rpx;
	}

	.person_name {
		font-size: 50rpx;
		font-weight: bold;
		width: 400rpx;
		display: -webkit-box;
		/*弹性伸缩盒子模型显示*/
		-webkit-box-orient: vertical;
		/*排列方式*/
		-webkit-line-clamp: 1;
		/*显示文本行数*/
		overflow: hidden;
		/*溢出隐藏*/
	}

	.person_follow {
		font-size: 30rpx;
		color: #5F5F5F;
	}

	.person_description {
		/* margin-top: 10rpx; */
		/* margin-left: 20rpx; */
		/* margin-right: 20rpx; */
		width: 400rpx;
		color: #909090;
	}

	.person_description text {
		width: 100%;
		font-size: 30rpx;
		display: -webkit-box;
		/*弹性伸缩盒子模型显示*/
		-webkit-box-orient: vertical;
		/*排列方式*/
		-webkit-line-clamp: 1;
		/*显示文本行数*/
		overflow: hidden;
		/*溢出隐藏*/
	}

	.information_card {
		margin: 0;
		padding: 10rpx;
	}

	.information_container {
		width: 95%;
	}

	.information_title {
		color: #606060;
		font-size: 30rpx;
	}

	.information_text {
		font-size: 30rpx;
	}

	.information_edit {
		height: 40rpx;
		width: 40rpx;
		margin-top: 30rpx;
	}

	.bottom_tips {
		margin-top: 30rpx;
		text-align: center;
		height: 200rpx;
	}

	.bottom_tips text {
		color: #A8A8A8;
		bottom: 0;
	}

	.bottom_button {
		width: 90%;
		margin: 30rpx 5%;
		box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
	}

	.function {
		background-image: linear-gradient(120deg, #a1c4fd 0%, #c2e9fb 100%);
	}

	.logout {
		background-image: linear-gradient(to right, #f78ca0 0%, #f9748f 19%, #fd868c 60%, #fe9a8b 100%);
	}

	.subscribe_icon {
		width: 20px;
		height: 20px;
		position: absolute;
		top: 6px;
		right: 18px;
	}
</style>
