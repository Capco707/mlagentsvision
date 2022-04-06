from mlagents_envs.environment import UnityEnvironment
from mlagents_envs.environment import ActionTuple
from mlagents_envs.side_channel.engine_configuration_channel import (
    EngineConfigurationChannel,
)
import numpy as np
import cv2 as cv
import time


def post_process_image(img_array):
    img = (img_array * 255.0).astype(np.uint8)
    img = cv.cvtColor(img, cv.COLOR_RGB2BGR)  # opencv default order is BGR
    return img


# create Engine Configuration Channel
channel = EngineConfigurationChannel()

# This is a non-blocking call that only loads the environment.
env = UnityEnvironment(file_name=None, seed=1, side_channels=[channel])

# Set the simulation configuration
channel.set_configuration_parameters(time_scale=100, capture_frame_rate=200)

# Start interacting with the environment.
for i in range(10):
    env.reset()
    obj_behavior_name = list(env.behavior_specs)[0]

    for j in range(200):
        obj_brain_info = env.get_steps(obj_behavior_name)

        obj_state = obj_brain_info[0]
        done = len(obj_brain_info[1].agent_id)
        if done:
            break

        original_img = obj_state.obs[2][0]  # RGB image
        seg_img = obj_state.obs[1][0]  # segmentation image
        depth_img = obj_state.obs[0][0]  # depth image

        depth_img = post_process_image(depth_img)
        original_img = post_process_image(original_img)
        seg_img = post_process_image(seg_img)
        if i == 0:
            cv.imwrite('img/test_%d.png' % i, original_img)
            cv.imwrite('img/test_depth_%d.png' % i, depth_img)
            cv.imwrite('img/test_seg_%d.png' % i, seg_img)

        action = 2 * np.random.rand(1, 2) - 1
        actionTuple = ActionTuple()
        actionTuple.add_continuous(action)
        print(actionTuple.continuous)
        env.set_actions(obj_behavior_name, actionTuple)

        env.step()
