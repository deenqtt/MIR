# debug_nav2_launch.py

from launch import LaunchDescription
from launch_ros.actions import Node

def generate_launch_description():
    return LaunchDescription([
        Node(
            package='nav2_bringup',
            executable='bringup_launch.py',
            output='screen',
            parameters=[{
                'use_sim_time': True
            }],
            arguments=['--ros-args', '--log-level', 'debug'],
            remappings=[('/tf', 'tf'),
                        ('/tf_static', 'tf_static')],
            extra_arguments=[{'use_intra_process_comms': True}]
        ),
        Node(
            package='nav2_controller',
            executable='controller_server',
            name='controller_server',
            output='screen',
            parameters=[{
                'use_sim_time': True,
                'controller_frequency': 10.0,
                'min_x_velocity_threshold': 0.001,
                'min_y_velocity_threshold': 0.5,
                'min_theta_velocity_threshold': 0.001,
                'failure_tolerance': 0.3,
                'progress_checker_plugin': 'progress_checker',
                'goal_checker_plugins': ['general_goal_checker'],
                'controller_plugins': ['FollowPath'],
                'progress_checker.required_movement_radius': 0.5,
                'progress_checker.movement_time_allowance': 10.0,
                'general_goal_checker.stateful': True,
                'general_goal_checker.plugin': 'nav2_controller::SimpleGoalChecker',
                'general_goal_checker.xy_goal_tolerance': 0.25,
                'general_goal_checker.yaw_goal_tolerance': 0.25,
                'FollowPath.plugin': 'dwb_core::DWBLocalPlanner',
                'FollowPath.debug_trajectory_details': True,
                'FollowPath.min_vel_x': 0.0,
                'FollowPath.min_vel_y': 0.0,
                'FollowPath.max_vel_x': 0.26,
                'FollowPath.max_vel_y': 0.0,
                'FollowPath.max_vel_theta': 1.0,
                'FollowPath.min_speed_xy': 0.0,
                'FollowPath.max_speed_xy': 0.26,
                'FollowPath.min_speed_theta': 0.0,
                'FollowPath.acc_lim_x': 2.5,
                'FollowPath.acc_lim_y': 0.0,
                'FollowPath.acc_lim_theta': 3.2,
                'FollowPath.decel_lim_x': -2.5,
                'FollowPath.decel_lim_y': 0.0,
                'FollowPath.decel_lim_theta': -3.2,
                'FollowPath.vx_samples': 20,
                'FollowPath.vy_samples': 5,
                'FollowPath.vtheta_samples': 20,
                'FollowPath.sim_time': 1.7,
                'FollowPath.linear_granularity': 0.05,
                'FollowPath.angular_granularity': 0.025,
                'FollowPath.transform_tolerance': 0.2,
                'FollowPath.xy_goal_tolerance': 0.25,
                'FollowPath.trans_stopped_velocity': 0.25,
                'FollowPath.short_circuit_trajectory_evaluation': True,
                'FollowPath.stateful': True,
                'FollowPath.critics': ['RotateToGoal', 'Oscillation', 'BaseObstacle', 'GoalAlign', 'PathAlign', 'PathDist', 'GoalDist'],
                'FollowPath.BaseObstacle.scale': 0.02,
                'FollowPath.PathAlign.scale': 32.0,
                'FollowPath.PathAlign.forward_point_distance': 0.1,
                'FollowPath.GoalAlign.scale': 24.0,
                'FollowPath.GoalAlign.forward_point_distance': 0.1,
                'FollowPath.PathDist.scale': 32.0,
                'FollowPath.GoalDist.scale': 24.0,
                'FollowPath.RotateToGoal.scale': 32.0,
                'FollowPath.RotateToGoal.slowing_factor': 5.0,
                'FollowPath.RotateToGoal.lookahead_time': -1.0,
            }],
            arguments=['--ros-args', '--log-level', 'debug']
        ),
    ])
