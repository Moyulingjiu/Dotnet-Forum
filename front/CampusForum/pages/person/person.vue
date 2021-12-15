<template>
	<view>
		<view class="person_card">
			<view class="flex_box">
				<image class="person_avater" :src="user.avater" mode="aspectFill"></image>
				<view class="person_information">
					<text class="person_name">{{ user.name }}</text>
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
				<view>
					<image class="information_edit" src="../../static/right.png" mode="aspectFill"></image>
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
				<view>
					<image class="information_edit" src="../../static/right.png" mode="aspectFill"></image>
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
				<view>
					<image class="information_edit" src="../../static/right.png" mode="aspectFill"></image>
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
				<view>
					<image class="information_edit" src="../../static/right.png" mode="aspectFill"></image>
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
				<view>
					<image class="information_edit" src="../../static/right.png" mode="aspectFill"></image>
				</view>
			</view>
			<view class="information_card flex_box">
				<view class="information_container">
					<view class="information_title">
						<text>生日</text>
					</view>
					<view class="information_text">
						<text>{{ user.brithday }}</text>
					</view>
				</view>
				<view>
					<image class="information_edit" src="../../static/right.png" mode="aspectFill"></image>
				</view>
			</view>
		</view>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				token: '',
				user: {
					studentId: 22920191234,
					name: '墨羽翎玖',
					collegeName: '信息学院',
					gender: 1,
					avater: '../../static/avater.jpg',
					description: '来时山有雪，归时雪满山。',
					brithday: '2021-12-15',
					phone: '12312341234',
					email: '123456@qq.com',
					following: 100,
					follower: 100
				}
			}
		},
		methods: {
			getGender() {
				if (this.user.gender == 0)
					return '保密'
				else if (this.user.gender == 1)
					return '男'
				return '女'
			}
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
	}

	.person_follow {
		font-size: 30rpx;
		color: #5F5F5F;
	}
	
	.person_description {
		/* margin-top: 10rpx; */
		/* margin-left: 20rpx; */
		/* margin-right: 20rpx; */
		color: #909090;
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
</style>
