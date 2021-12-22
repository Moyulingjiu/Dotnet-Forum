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
							<image class="user_avater" :src="item.avater" @click="userdetail(item.id)"></image>
							<view class="user" @click="userdetail(item.id)">
								<text class="user_name">{{ item.name }}</text>
								<br />
								<text class="user_description">{{ item.description }}</text>
							</view>
							<view :class="item.follow?'follow':'unfollow'" @click="changeFollow(index)">
								{{ item.follow?'已关注':'关注' }}
							</view>
						</view>
					</view>
					<view class="bottom_tips">
						<text>{{ (followingPage >= followingTotal - 1) ? bottomTipsNoMore : bottomTips }}</text>
					</view>
				</scroll-view>
			</swiper-item>
			<swiper-item>
				<scroll-view>
					<view v-for="(item,index) in follower">
						<view class="person_container">
							<image class="user_avater" :src="item.avater" @click="userdetail(item.id)"></image>
							<view class="user" @click="userdetail(item.id)">
								<text class="user_name">{{ item.name }}</text>
								<br />
								<text class="user_description">{{ item.description }}</text>
							</view>
							<view :class="item.follow?'follow':'unfollow'" @click="changeFollow(index)">
								{{ item.follow?'已关注':'关注' }}
							</view>
						</view>
					</view>
					<view class="bottom_tips">
						<text>{{ (followerPage >= followerTotal - 1) ? bottomTipsNoMore : bottomTips }}</text>
					</view>
				</scroll-view>
			</swiper-item>
		</swiper>

		<uni-popup ref="popup_success_refresh" type="message">
			<uni-popup-message type="success" message="刷新成功" :duration="2000"></uni-popup-message>
		</uni-popup>
	</view>
</template>

<script>
	import * as userApi from "../../api/user.js"
	import * as config from "../../utils/config.js"

	export default {
		data() {
			return {
				bottomTips: '- 上拉加载更多 -', // 底部提示
				bottomTipsNoMore: '- 到底了 -', // 底部提示
				curr: 0,
				follower: [ // 粉丝
					{
						id: 1,
						name: '用户',
						avater: '../../static/avater.jpg',
						description: '这个人没有个性签名',
						follow: true
					}
				],
				followerPage: 0,
				followerTotal: 1,
				following: [],
				followingPage: 0,
				followingTotal: 1,
				loadLock: false // 上拉加载的锁
			}
		},
		onLoad(options) {
			this.curr = options.index
			if (typeof this.curr == 'undefined' || this.curr > 1 || this.curr < 0) {
				this.curr = 0
			}
		},
		onShow() {
			this.refresh()
		},
		onPullDownRefresh() {
			this.refresh()
			uni.stopPullDownRefresh();
			this.$refs.popup_success_refresh.open('top')
		},
		onReachBottom() {
			if (!this.loadLock) { // 加锁
				this.loadLock = true
				if (this.curr == 0) {
					if (this.followingPage < this.followingTotal - 1) {
						this.loadData(this.followerPage, this.followingPage + 1)
					}
				} else {
					if (this.followerPage < this.followerTotal - 1) {
						this.loadData(this.followerPage + 1, this.followingPage)
					}
				}
				this.loadLock = false
			}
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
					this.followerPage = -1
					this.followerTotal = 1
					this.following = []
					this.followingPage = -1
					this.followingTotal = 1
					this.loadData(0, 0)
				}
			},
			loadData(followerPage = this.followerPage, followingPage = this.followingPage) {
				if (followerPage != this.followerPage) {
					this.followerPage = followerPage
					userApi.followers(followerPage).then(data => {
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
								let userItem = {
									id: data.data.items[key].id,
									name: data.data.items[key].name,
									avater: data.data.items[key].avater,
									description: data.data.items[key].description,
									follow: data.data.items[key].follow
								}
								this.follower.push(userItem)
							}
						}
					})
				}
				if (followingPage != this.followingPage) {
					this.followingPage = followingPage
					userApi.followings(followingPage).then(data => {
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
								let userItem = {
									id: data.data.items[key].id,
									name: data.data.items[key].name,
									avater: data.data.items[key].avater,
									description: data.data.items[key].description,
									follow: data.data.items[key].follow
								}
								this.following.push(userItem)
							}
						}
					})
				}
			},
			userdetail(userId) {
				uni.navigateTo({
					url: `/pages/otherUsers/otherUsers?id=${userId}`
				})
			},
			changeFollow(index) {
				let userId = 0
				let follow = false
				if (this.curr == 0) {
					userId = this.following[index].id
					follow = this.following[index].follow
				} else {
					userId = this.follower[index].id
					follow = this.follower[index].follow
				}
				if (follow) {
					userApi.unfollow(userId).then(data => {
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
							if (this.curr == 0) {
								this.following[index].follow = false
							} else {
								this.follower[index].follow = false
							}
						}
					})
				} else {
					userApi.follow(userId).then(data => {
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
							if (this.curr == 0) {
								this.following[index].follow = true
							} else {
								this.follower[index].follow = true
							}
						}
					})
				}
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
	
	.bottom_tips {
		margin-top: 30rpx;
		text-align: center;
		height: 200rpx;
	}

	.bottom_tips text {
		color: #A8A8A8;
		bottom: 0;
	}
</style>
