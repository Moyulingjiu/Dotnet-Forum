<template>
	<view class="big">
		<view class="trade">
			<view class="texts" :class="curr==0?'active':''" data-index="0" @tap="setCurr">
				关注
			</view>
			<view class="texts" :class="curr==1?'active':''" data-index="1" @tap="setCurr">
				粉丝
			</view>
		</view>
		<swiper :current="curr" @change="setCurr">
			<swiper-item>
				<scroll-view>
					<view v-for="(item,index) in following">
						<view class="person_container">
							<image class="user_avater" src="../../static/avater.jpg"></image>
							<view class="user">
								<text class="user_name">{{ item.name }}</text>
								<br />
								<text class="user_description">{{ item.description }}</text>
							</view>
							<view :class="item.isFollow?'follow':'unfollow'">
								{{ item.isFollow?'已关注':'关注' }}
							</view>
						</view>
					</view>
				</scroll-view>
			</swiper-item>
			<swiper-item>
				<scroll-view>
					<view v-for="(item,index) in follower">
						<view class="person_container">
							<image class="user_avater" src="../../static/avater.jpg"></image>
							<view class="user">
								<text class="user_name">{{ item.name }}</text>
								<br />
								<text class="user_description">{{ item.description }}</text>
							</view>
							<view :class="item.isFollow?'follow':'unfollow'">
								{{ item.isFollow?'已关注':'关注' }}
							</view>
						</view>
					</view>
				</scroll-view>
			</swiper-item>
		</swiper>
	</view>
</template>

<script>
	import * as userApi from "../../api/user.js"
	import * as config from "../../utils/config.js"

	export default {
		data() {
			return {
				curr: 0,
				follower: [ // 粉丝
					{
						id: 1,
						name: '用户',
						avater: '../../static/avater.jpg',
						description: '这个人没有个性签名',
						isFollow: true
					},
					{
						id: 1,
						name: '用户',
						avater: '../../static/avater.jpg',
						description: '这个人没有个性签名',
						isFollow: false
					}
				],
				followerPage: 0,
				followerTotal: 1,
				following: [
					{
						id: 1,
						name: '用户',
						avater: '../../static/avater.jpg',
						description: '这个人没有个性签名',
						isFollow: true
					},
					{
						id: 1,
						name: '用户',
						avater: '../../static/avater.jpg',
						description: '这个人没有个性签名',
						isFollow: true
					}
				],
				followingPage: 0,
				followingTotal: 1
			}
		},
		onLoad(options) {
			this.curr = options.index
			if (typeof this.curr == 'undefined' || this.curr > 1 || this.curr < 0) {
				this.curr = 0
			}
		},
		onShow() {
			// this.refresh()
		},
		methods: {
			setCurr(e) {
				let thisCurr = e.detail.current || e.currentTarget.dataset.index || 0;
				this.curr = thisCurr;
			},
			refresh() {
				if (!config.checkToken()) {
					uni.redirectTo({
						url: '../login/login'
					})
				} else {
					this.follower = []
					this.followerPage = 0
					this.followerTotal = 1
					this.following = []
					this.followingPage = 0
					this.followingTotal = 1
					this.loadData()
				}
			},
			loadData(followerPage = this.followerPage, followingPage = this.followingPage) {
				userApi.followers(followerPage).then(data => {
					let followerId = []
					if (typeof data === "undefined") {
						uni.showToast({
							title: '服务器错误',
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
						this.followerTotal = data.data.total
						for (let key in data.data.items) {
							if (key != 'length') {
								followerId.push(data.data.items[key])
							}
						}
					}
					return followerId
				}).then(data => {
					for (let key in data) {
						userApi.select(data[key]).then(information => {
							console.log(information)
							if (typeof information === "undefined") {
								uni.showToast({
									title: '服务器错误',
									icon: "error",
									mask: true,
									duration: 2000
								})
							} else if (information.code != 200) {
								uni.showToast({
									title: information.msg,
									icon: "error",
									mask: true,
									duration: 2000
								})
							} else {}
						})
					}
				})
				userApi.followings(followingPage).then(data => {
					let followingId = []
					if (typeof data === "undefined") {
						uni.showToast({
							title: '服务器错误',
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
						this.followingTotal = data.data.total
						for (let key in data.data.items) {
							if (key != 'length') {
								followingId.push(data.data.items[key])
							}
						}
					}
					return followingId
				}).then(data => {
					// console.log(data)
				})

			}
		}
	}
</script>

<style>
	.trade {
		margin-top: 30rpx;
		width: 100%;
		overflow: auto;
	}

	.trade view {
		text-align: center;
		width: 50%;
		float: left;
		font-size: 40rpx;
		font-weight: bold;
		padding-bottom: 10rpx;
	}

	.trade .texts.active {
		border-bottom: 8rpx solid #00A1D6;
	}

	.person_container {
		margin-bottom: 20rpx;
		padding: 10rpx;
		display: flex;
	}

	.user_avater {
		width: 100rpx;
		height: 100rpx;
		border-radius: 50%;
	}

	.user {
		margin-left: 15rpx;
	}

	.user_name {
		font-size: 45rpx;
		font-weight: bold;
	}

	.user_description {
		font-size: 35rpx;
		color: #555555;
	}

	.unfollow {
		position: absolute;
		right: 30rpx;
		font-size: 40rpx;
		color: white;
		margin-top: 30rpx;
		width: 200rpx;
		height: 60rpx;
		text-align: center;
		background: #83cbac;
	}

	.follow {
		position: absolute;
		right: 30rpx;
		font-size: 40rpx;
		color: white;
		margin-top: 30rpx;
		width: 200rpx;
		height: 60rpx;
		text-align: center;
		background: #b5aa90;
		/* box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19); */
	}
</style>
